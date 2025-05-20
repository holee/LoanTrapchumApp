using System.Data;
using Applications.Constants.Tables;
using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.Disbursements;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Lists.Customers;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class LoanService(
        DataContext context,
        ILogger<LoanService> logger,
        IDapperConnectionFactory connectionFactory)
        : ILoanService
    {
        public async Task<Result<string>> CreateLoanAsync(LoanDtoCreate req)
        {
            await using var trans = await context.Database.BeginTransactionAsync();

            try
            {
                var customerInActiveLoan = await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CustomerId == req.CustomerRefId && c.IsLoanActive == true);

                if (customerInActiveLoan is not null)
                {
                    return Result.Failure<string>(ErrorResponse.Conflict("This Customer Is Already disburse"));
                }

                var tranx = new LoanTransaction
                {
                    TransactionsId = $"trx-{Guid.CreateVersion7()}",
                    UserRefId = req.UserRefId,
                    TransactionTypeName = "Loan Disbursement",
                    TranTypeRefId = 19,
                    IsActive = true
                };

                var cashMgt = new CashManagement
                {
                    Id = $"cash-{Guid.CreateVersion7()}",

                    BankRefId = req.BankRefId,

                    BranchRefId = req.BranchId,

                    Amount = req.PaidAmount,

                    CurrencyRefId = req.CurrencyRefId,

                    CashStatusRefId = 2,

                    Remark = "Cash goes through transaction",

                    TransactionDate = req.RegisterDate,

                    BusinessDate = req.RegisterDate,

                    Balance = 0,

                    EmployeeRefId = req.UserRefId
                };

                tranx.CashManagements.Add(cashMgt);

                var model = req.MapLoan();

                model.LoanTransaction = tranx;

                context.LoanDisbursments.Add(model);

                await context.SaveChangesAsync();

                await context.Customers.Where(x => x.CustomerId == req.CustomerRefId)
                    .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

                await context.LoanRequests.Where(x => x.LoanRequestId == req.LoanRequestId &&
                                                      x.CustomerRefId == req.CustomerRefId)
                    .ExecuteUpdateAsync(prop => prop
                        .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Completed)
                        .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

                await trans.CommitAsync();

                return Result.Success(new string("New Loan Disbursement was added."));
            }
            catch (DbUpdateException ex)
            {
                await trans.RollbackAsync();

                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<Result<bool>> CreateLoanAsync(LoanDisbursment req)
        {
            await using var trans = await context.Database.BeginTransactionAsync();

            try
            {
                if (await CustomerAlreadyRequest(req.CustomerRefId))
                {
                    return Result.Failure<bool>(ErrorResponse.Conflict("This Customer Is Already Requested"));
                }

                if (await CustomerInActiveLoan(req.CustomerRefId))
                {
                    return Result.Failure<bool>(ErrorResponse.Conflict("This Customer Is Already disburse"));
                }

                context.LoanDisbursments.Add(req);

                await context.SaveChangesAsync();

                await context.Customers.Where(x => x.CustomerId == req.CustomerRefId)
                    .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

                await context.LoanRequests.Where(x => x.LoanRequestId == req.LoanRequestId &&
                                                      x.CustomerRefId == req.CustomerRefId)
                    .ExecuteUpdateAsync(prop => prop
                        .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Completed)
                        .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

                await trans.CommitAsync();

                return Result.Success<bool>(true);
            }
            catch (DbUpdateException ex)
            {
                await trans.RollbackAsync();

                return Result.Failure<bool>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }

        private async Task<bool> CustomerAlreadyRequest(string? customerId)
        {
            if (string.IsNullOrEmpty(customerId)) return false;

            using var db = await connectionFactory.CreateConnectionAsync();

            var parameter = new DynamicParameters();

            parameter.Add("@CustomerId", customerId);

            var sql =
                $@"SELECT LoanRequestId,RequestAmount,CustomerId FROM VLoanRequests WHERE CustomerId=@CustomerId AND StatusId NOT IN (2,3,4)";

            var existingLoanRequest = await db.QueryAsync<VLoanRequests>(sql, parameter);

            return existingLoanRequest.Any();
        }

        private async Task<bool> CustomerInActiveLoan(string? customerId)
        {
            if (string.IsNullOrEmpty(customerId)) return false;

            using var db = await connectionFactory.CreateConnectionAsync();

            var parameter = new DynamicParameters();

            parameter.Add("@CustomerId", customerId);

            parameter.Add("@IsLoanActive", true);
            var sql =@"SELECT 
                            CustomerId,
                            FirstName,
                            LastName,
                            IsLoanActive 
                     FROM VCustomerList 
                     WHERE CustomerId=@CustomerId AND IsLoanActive=@IsLoanActive";
            var customerInActiveLoan = await db.QueryAsync<VCustomerList>(sql, parameter);
            return customerInActiveLoan.Any();
        }


        public async Task<Result<string>> DeleteAsync(string? loanId, int? branchId)
        {
            var loan = context.LoanDisbursments.Where(x => x.LoanId == loanId && x.BranchRefId == branchId)
                .FirstOrDefault();

            if (loan is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("No Loan Data."));
            }

            context.LoanDisbursments.Remove(loan);

            await context.SaveChangesAsync();

            return Result.Success(new string("data wad delete."));
        }

        public async Task<Result<string>> UpdateAsync(LoanDisbursment disbursement)
        {
            context.LoanDisbursments.Update(disbursement);

            await context.SaveChangesAsync();

            return Result.Success(new string("New Loan Disbursement was added."));
        }

        public async Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req)
        {
            var exstingLoan = context.LoanDisbursments.Where(x =>
                x.LoanId == req.LoanRefId && x.BranchRefId == branchId &&
                x.CreditAgentRefId == req.OldCreditAgentRefId);

            if (!exstingLoan.Any())
            {
                return Result.Failure<string>(ErrorResponse.NotFound("No Loan Data found"));
            }

            var trx = context.Database.BeginTransaction();

            try
            {
                var model = req.Adapt<LoanChangedCa>();

                context.LoanChangedCa.Add(model);

                await context.SaveChangesAsync();
                var editSeq = exstingLoan.First().EditSeq + 1;
                context.LoanDisbursments
                    .Where(x => x.LoanId == req.LoanRefId &&
                                x.BranchRefId == branchId &&
                                x.CreditAgentRefId == req.OldCreditAgentRefId)
                    .ExecuteUpdate(prop => prop.SetProperty(p => p.CreditAgentRefId, req.NewCreditAgentRefId)
                        .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                        .SetProperty(p => p.EditSeq, editSeq));

                trx.Commit();

                return Result.Success<string>(new("Loan has changed new credit agent."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.InnerException!.Message);

                trx.Rollback();

                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCaDtoCreate> req)
        {
            var trx = context.Database.BeginTransaction();
            try
            {
                var model = req.Adapt<IEnumerable<LoanChangedCa>>();

                context.LoanChangedCa.AddRange(model);

                await context.SaveChangesAsync();

                var oldCreditAgentId = req.First().OldCreditAgentRefId;

                var newCreditAgentRefId = req.First().NewCreditAgentRefId;

                context.LoanDisbursments
                    .Where(x => x.BranchRefId == branchId && x.CreditAgentRefId == oldCreditAgentId)
                    .ExecuteUpdate(prop => prop.SetProperty(p => p.CreditAgentRefId, newCreditAgentRefId)
                        .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

                trx.Commit();

                return Result.Success<string>(new("Loan has changed new credit agent."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.InnerException!.Message);
                trx.Rollback();
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Pager<VLoanDisbursments>> GetLoansAsync(int? branchId, string? creditAgentId,
            LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
        {
            using var db = connectionFactory.CreateConnection();

            var queries = await db.QueryAsync<VLoanDisbursments>("spGetLoansByBranchId", new
            {
                @BranchId = branchId,
                @CreditAgentId = creditAgentId,
                @LoanStatusId = (int)status,
                @SortColumn = param?.SortColumn,
                @SortDescending = param?.SortDescending,
                @PageIndex = param?.PageNumber ?? 1,
                @PageSize = param?.PageSize ?? 25,
                @Filter = param?.Filter ?? string.Empty
            }, commandType: CommandType.StoredProcedure);

            var counts = queries.Count();

            var items = queries.ToList();

            return new Pager<VLoanDisbursments>(items, counts);
        }

        public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId,
            ScheduleStatus status, bool include = false)
        {
            using var db = connectionFactory.CreateConnection();

            if (include)
            {
                var jointDictionary = new Dictionary<string, VLoanDisbursments>();

                var queries = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>(ProcedureConstant.GetLoanById,
                    (disbursment, schedule) =>
                    {
                        if (jointDictionary.TryGetValue(disbursment.LoanId!, out var existingDis))
                        {
                            disbursment = existingDis!;
                        }
                        else
                        {
                            jointDictionary.Add(disbursment.LoanId!, disbursment);
                        }

                        disbursment.Schedules?.Add(schedule);

                        return disbursment;
                    },
                    param: new
                    {
                        @LoanId = loanId,

                        @BranchId = branchId,

                        @ScheduleStatusId = (int)status,

                        @Include = include
                    }, splitOn: "LoanScheduleId"
                    , commandType: CommandType.StoredProcedure);

                var item = queries.FirstOrDefault();

                if (item is null)
                {
                    return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
                }

                return Result.Success(item);
            }

            var result = await db.QueryFirstOrDefaultAsync<VLoanDisbursments>("spGetLoanById",
                param: new
                {
                    @LoanId = loanId,

                    @BranchId = branchId,

                    @ScheduleStatusId = (int)status,

                    @Include = include
                }, commandType: CommandType.StoredProcedure);


            if (result is null)
            {
                return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
            }

            return Result.Success(result);
        }

     public async Task<Result<VLoanDisbursments>> GetLoanWithSchedulesByIdAsync(string? loanId,int? branchId,ScheduleStatus status,bool include = false)
        {
            using var db = connectionFactory.CreateConnection(); 

            var parameters = new
            {
                LoanId = loanId,
                BranchId = branchId,
                ScheduleStatusId = (int)status,
                Include = include
            };

            if (include)
            {
                var loanMap = new Dictionary<string, VLoanDisbursments>();

                var queries = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>("spGetLoanById" ,
                    (disbursment, schedule) =>
                    {
                        if (!loanMap.TryGetValue(disbursment.LoanId!, out var existing))
                        {
                            disbursment.Schedules = new List<VSchedule>();

                            loanMap[disbursment.LoanId!] = disbursment;

                            existing = disbursment;
                        }

                        if (schedule != null)
                        {
                            existing.Schedules?.Add(schedule);
                        }

                        return existing;
                    },
                    param: parameters,
                    splitOn: "LoanScheduleId",
                    commandType: CommandType.StoredProcedure
                );

                var resultItem = loanMap.Values.FirstOrDefault();

                if (resultItem is null)
                {
                    return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No data was found."));
                }

                return Result.Success(resultItem);
            }

            var result = await db.QueryFirstOrDefaultAsync<VLoanDisbursments>(
                "spGetLoanById",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            if (result is null)
            {
                return Result.Failure<VLoanDisbursments>(
                    ErrorResponse.NotFound("No data was found."));
            }

            return Result.Success(result);
        }


    }
}

//public async Task<Result<VSchedule>> GetPaymentScheduleById(string? loanId, long? loanScheduleId, int? branchId, bool related)
//{

//    var schedule = await _context.Database.SqlQuery<VSchedule>($"SELECT * FROM  VSchedules WHERE LoanId={loanId} AND  BranchId={branchId} AND LoanScheduleId={loanScheduleId}")
//                                          .FirstOrDefaultAsync();
//    if (schedule is null)
//    {
//        return Result.Failure<VSchedule>(ErrorResponse.NotFound("No Schedule Found."));
//    }

//    if (related)
//    {

//        var paymentLines = await _context.Database.SqlQuery<VPaymentLines>($"SELECT * FROM VLoanRepaymentLines WHERE LoanId={loanId} AND LoanScheduleRefId={loanScheduleId}")
//                                        .ToListAsync();

//        schedule.PaymentLines = paymentLines;

//    }

//    return Result.Success(schedule);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = true, string? filter = null)
//{
//    using var db = _connectionFactory.CreateConnection();

//    var schedules = await db.QueryAsync<VSchedule>("spGetCustomerPayementSchedules", new
//    {
//        paymentDate,
//        branchId,
//        includeLateSchedule,
//        filter
//    }, commandType: CommandType.StoredProcedure);
//    if (!schedules.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Payment schedule found."));
//    }
//    return Result.Success(schedules);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial 
//    var db = _connectionFactory.CreateConnection();

//    var items = await db.QueryAsync<VSchedule>("spGetSchedulesById", new
//    {
//        branchId,
//        loanId,
//        @statusId = (int)status
//    }, commandType: CommandType.StoredProcedure);

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Schedule Found."));
//    }
//    return Result.Success(items);
//}


//private async Task<Result<VLoanDisbursments>> GetLoanByIdWithSchedulesAsync(string? loanId, int? branchId, ScheduleStatus status)
//{
//    using var db = _connectionFactory.CreateConnection(); 

//    var jointDictionary = new Dictionary<string, VLoanDisbursments>();

//    var queries = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>("spGetLoanById",

//             (disbursment, schedule) =>
//             {
//                 if (jointDictionary.TryGetValue(disbursment.LoanId!, out var existingDis))
//                 {
//                     disbursment = existingDis!;
//                 }
//                 else
//                 {
//                     jointDictionary.Add(disbursment.LoanId!, disbursment);
//                 }

//                 disbursment.Schedules?.Add(schedule);

//                 return disbursment;
//             },
//            param: new
//            {
//                @LoanId = loanId,

//                @BranchId = branchId,

//                @ScheduleStatusId = (int)status,

//            }, splitOn: "LoanScheduleId"
//            , commandType: CommandType.StoredProcedure);

//    var item = queries.FirstOrDefault();

//    if (item is null)
//    {
//        return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
//    }

//    return Result.Success(item);
//}


//private async Task<Result<VLoanDisbursments>> GetLoanByIdOnlyAsync(string? loanId, int? branchId, ScheduleStatus status)
//{
//    using var db = _connectionFactory.CreateConnection();

//    var item = await db.QueryFirstAsync<VLoanDisbursments>(@"SELECT * FROM VLoansDisbursments WHERE LoanId=@LoanId AND
//                                                             BranchId=@BranchId AND ScheduleStatusId=@ScheduleStatusId
//                                                             ",
//            param: new
//            {
//                @LoanId = loanId,

//                @BranchId = branchId,

//                @ScheduleStatusId = (int)status,

//            });

//    if (item is null)
//    {
//        return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
//    }

//    return Result.Success(item);
//}


////var items = _context.Database
////                    .SqlQuery<VLoanDisbursments>($"""
////                                                  SELECT * FROM VLoanDisbursments WHERE 
////                                                  ({branchId} > 0 AND BranchId={branchId} OR {branchId}=0) AND LoanStatusId={status}
////                                                  AND ({creditAgentId!.Length}> 0 AND CreditAgentId={creditAgentId} OR {creditAgentId!.Length}=0
////                                                  """);

////if (!string.IsNullOrEmpty(param?.Filter))
////{

////}