using System.Data;
using Applications.Enums;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Infrastructures.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreBussiness.Implements
{
    public class LoanService : ILoanServiceImpl
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoanService> _logger;
        private readonly IDapperConnectionFactory _connectionFactory;
        public LoanService(ApplicationDbContext context, ILogger<LoanService> logger, IDapperConnectionFactory connectionFactory)
        {
            _context = context;
            _logger = logger;
            _connectionFactory = connectionFactory;
        }
        public async Task<Result<string>> CreateLoanAsync(LoanDtoCreate req)
        {

            using var trans = _context.Database.BeginTransaction();

            try
            {
                var customerInActiveLoan = _context.Customers
                                                .Where(c => c.CustomerId == req.CustomerRefId && c.IsLoanActive == true)
                                                .AsNoTracking();

                if (customerInActiveLoan.Any())
                {
                    return Result.Failure<string>(ErrorResponse.Conflict("this customer is in loan"));
                }

                var tranx = new LoanTransaction
                {
                    UserRefId = req.UserRefId,
                    TransactionTypeName = "Loan Disbursment",
                    TranTypeRefId = 19,
                    IsActive = true
                };

                //var totalPrinciplePayment = req.Principle - req.AdminFeeAmount;

                var cashMgt = new CashManagement
                {
                    BankRefId = req.BankRefId,
                    BranchRefId = req.BranchRefId,
                    Amount = req.PaidAmount,
                    CurrencyRefId = req.CurrencyRefId,
                    CashStatusRefId = 2,
                    Remark = "Cash goes through transaction",
                    StaffRefId = req.UserRefId,
                    Receive = "N",
                    Reject = "N",
                    TransactionDate = req.RegisterDate,
                    BusinessDate = req.RegisterDate,
                    Balance = 0
                };
                tranx.CashManagements.Add(cashMgt);
                //_context.LoanTransaction.Add(tranx);
                var model = req.Adapt<Loan>();

                model.LoanTransaction = tranx;

                _context.LoanDisbursments.Add(model);

                await _context.SaveChangesAsync();

                await _context.Customers.Where(x => x.CustomerId == req.CustomerRefId)
                                        .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

                await _context.LoanRequests.Where(x => x.Id == req.LoanRequestId && x.CustomerRefId == req.CustomerRefId)
                                           .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, 7));


                //await _context.SaveChangesAsync();

                trans.Commit();

                return Result.Success(new string("New Loan Disbursement was added."));
            }
            catch (DbUpdateException ex)
            {
                trans.Rollback();

                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

            }
        }
        public async Task<Result<string>> DeleteAsync(long loanId, int branchId)
        {
            var loan = _context.LoanDisbursments.Where(x => x.Id == loanId && x.BranchRefId == branchId).FirstOrDefault();
            if (loan is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("No Loan Data."));
            }
            _context.LoanDisbursments.Remove(loan);

            await _context.SaveChangesAsync();

            return Result.Success(new string("data wad delete."));
        }
        public async Task<Result<string>> UpdateAsync(Loan disbursement)
        {
            _context.LoanDisbursments.Update(disbursement);
            await _context.SaveChangesAsync();
            return Result.Success(new string("New Loan Disbursement was added."));
        }
        public async Task<Result<string>> ChangedCreditAgent(int branchId, LoanChangedCaDtoCreate req)
        {

            var exstingLoan = _context.LoanDisbursments.Where(x => x.Id == req.LoanRefId && x.BranchRefId == branchId && x.CreditAgentRefId == req.OldCreditAgentRefId);

            if (!exstingLoan.Any())
            {
                return Result.Failure<string>(ErrorResponse.NotFound("No Loan Data found"));
            }
            var trx = _context.Database.BeginTransaction();
            try
            {
                var model = req.Adapt<LoanChangedCa>();

                _context.LoanChangedCa.Add(model);

                await _context.SaveChangesAsync();
                var editSeq = exstingLoan.First().EditSeq + 1;
                _context.LoanDisbursments
                        .Where(x => x.Id == req.LoanRefId &&
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
                _logger.LogError(ex, ex.InnerException!.Message);
                trx.Rollback();
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
            }
        }
        public async Task<Result<string>> ChangedCreditAgents(int branchId, List<LoanChangedCaDtoCreate> req)
        {
            var trx = _context.Database.BeginTransaction();
            try
            {
                var model = req.Adapt<IEnumerable<LoanChangedCa>>();

                _context.LoanChangedCa.AddRange(model);

                await _context.SaveChangesAsync();

                var oldCreditAgentId = req.First().OldCreditAgentRefId;
                var newCreditAgentRefId = req.First().NewCreditAgentRefId;
                _context.LoanDisbursments
                        .Where(x => x.BranchRefId == branchId && x.CreditAgentRefId == oldCreditAgentId)
                        .ExecuteUpdate(prop => prop.SetProperty(p => p.CreditAgentRefId, newCreditAgentRefId)
                                                   .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

                trx.Commit();

                return Result.Success<string>(new("Loan has changed new credit agent."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException!.Message);
                trx.Rollback();
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
            }


        }
        public async Task<Result<VSchedule>> GetPaymentScheduleById(long loanId, long loanScheduleId, int branchId, bool related)
        {

            var schedule = await _context.Database.SqlQuery<VSchedule>($"SELECT * FROM  VSchedules WHERE LoanId={loanId} AND  BranchId={branchId} AND LoanScheduleId={loanScheduleId}")
                                                  .FirstOrDefaultAsync();
            if (schedule is null)
            {
                return Result.Failure<VSchedule>(ErrorResponse.NotFound("No Schedule Found."));
            }

            if (related)
            {

                var paymentLines = await _context.Database.SqlQuery<VPaymentLines>($"SELECT * FROM VLoanRepaymentLines WHERE LoanId={loanId} AND LoanScheduleRefId={loanScheduleId}")
                                                .ToListAsync();

                schedule.PaymentLines = paymentLines ?? [];

            }

            return Result.Success<VSchedule>(schedule);
        }


        public async Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = true, string? filter = null)
        {
            using var db = _connectionFactory.CreateConnection();

            var schedules = await db.QueryAsync<VSchedule>("spGetCustomerPayementSchedules", new
            {
                paymentDate,
                branchId,
                includeLateSchedule,
                filter
            }, commandType: CommandType.StoredProcedure);
            if (!schedules.Any())
            {
                return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Payment schedule found."));
            }
            return Result.Success(schedules);
        }



        public async Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
        {
            //0 all
            //1 not paid
            //2 only paid
            //3 only partial 
            var db = _connectionFactory.CreateConnection();
            var items = await db.QueryAsync<VSchedule>("spGetSchedulesById", new
            {
                branchId,
                loanId,
                @statusId = (int)status
            }, commandType: CommandType.StoredProcedure);

            if (!items.Any())
            {
                return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Schedule Found."));
            }
            return Result.Success(items);
        }


        public async Task<Pager<VLoanDisbursments>> GetLoansAsync(int branchId, long creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
        {

            using var db = _connectionFactory.CreateConnection();
            var queries = await db.QueryAsync<VLoanDisbursments>("spGetLoansByBranchId", new
            {
                @BranchId = branchId,
                @CreditAgentId = creditAgentId,
                @LoanStatusId = (int)status,
                @SortColumn = param?.SortColumn,
                @SortDescending = param?.SortDescending,
                @PageIndex = param?.PageIndex ?? 1,
                @PageSize = param?.PageSize ?? 25,
                @Filter = param?.Filter ?? string.Empty
            }, commandType: CommandType.StoredProcedure);

            var counts = queries.Count();

            var items = queries.ToList();

            return new Pager<VLoanDisbursments>(items, counts);


        }
        public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(long loanId, int branchId, ScheduleStatus status)
        {
            using var db = _context.Database.GetDbConnection();

            var queries = await db.QueryMultipleAsync("spGetLoanById",
            new
            {
                @LoanId = loanId,
                @BranchId = branchId,
                @ScheduleStatusId = (int)status,
            }, commandType: CommandType.StoredProcedure);

            var existongLoan = await queries.ReadFirstOrDefaultAsync<VLoanDisbursments>();

            if (existongLoan is null)
            {
                return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No data found."));
            }

            var schedules = await queries.ReadAsync<VSchedule>();

            existongLoan.Schedules = schedules.ToList() ?? [];

            return Result.Success(existongLoan);
        }

    }
}

//IQueryable<VLoans> queries = _context.Database.SqlQuery<VLoans>(@$"SELECT * FROM VLoanDisbursments d WHERE d.BranchId={branchId}");

//if (!queries.Any())
//{
//    return new Pager<VLoans>();
//}

//if (!string.IsNullOrEmpty(param.Filter))
//{
//    queries = queries.Where(p => EF.Functions.Like(p.FirstName, $"%{param.Filter}%") ||
//                             EF.Functions.Like(p.LastName, $"%{param.Filter}%") ||
//                             EF.Functions.Like(p.AltName, $"%{param.Filter}%") ||
//                             EF.Functions.Like(p.AgentName, $"%{param.Filter}%") ||
//                             EF.Functions.Like(p.Phone, $"%{param.Filter}%"));
//}

//if (creditAgentId > 0)
//{
//    queries = queries.Where(x => x.CreditAgentRefId == creditAgentId && x.BranchId == branchId);
//}

//if (!param.IsIncludeInActive)
//{
//    queries = queries.Where(p => p.LoanstatusId == 4 && p.BranchId == branchId);
//}

//if (param.SortDescending)
//{
//    queries = queries.OrderByDescending(p => EF.Property<object>(p, param.SortColumn));
//}
//var counts = await queries.CountAsync();

//var items = await queries.Skip(param.PageIndex)
//                            .Take(param.PageSize)
//                            .AsNoTracking()
//                            .ToListAsync();
//return new Pager<VLoans>(items, counts);




//public async Task<Result<Loan>> GetLoanByIdAsync(long loanId, int branchId, params string[] includes)
//{
//    IQueryable<Loan> queries = _context.LoanDisbursments
//                                        .Where(x => x.Id == loanId &&
//                                                    x.BranchRefId == branchId);

//    if (includes.Any())
//    {

//        foreach (var prop in includes)
//        {
//            queries = queries.Include(prop);
//        }
//    }

//    if (!queries.Any())
//    {

//        return Result.Failure<Loan>(Error.NotFound("404", ["No data found"]));

//    }

//    var query = await queries.FirstOrDefaultAsync();

//    return Result.Success(query!);

//}













//public async Task<Result<IEnumerable<VSchedule>>> GetLatePaymentSchedules(int? branchId, DateTime? paymentDate = null, string? filter = null)
//{

//    using var db = _connectionFactory.CreateConnection();

//    string sql = "SELECT * FROM VSchedules WHERE LoanId=@LoanId AND BranchId=@BranchId AND IsLate=@IsLate";

//    var items = await db.QueryAsync<VSchedule>(sql, new { branchId, paymentDate, filter });

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    return Result.Success(items);
//}


//private async Task<Result<VLoans>> GetLoanDetailWithRelation(long loanId, int branchId)
//{
//    using var db = _context.Database.GetDbConnection();

//    var loanDictionary = new Dictionary<long, VLoans>();

//    var existongLoan = await db.QueryAsync<VLoans, VSchedule, VLoans>("spGetLoanById", (vLoans, vSchedule) =>
//                                {

//                                    if (loanDictionary.TryGetValue(vLoans.Id!.Value, out var existingVloan))
//                                    {
//                                        vLoans = existingVloan;
//                                    }
//                                    else
//                                    {
//                                        loanDictionary.Add(vLoans.Id.Value, vLoans);
//                                    }

//                                    vLoans.Schedules?.Add(vSchedule);

//                                    return vLoans;
//                                },
//    new
//    {
//        @LoanId = loanId,
//        @BranchId = branchId
//    }, commandType: CommandType.StoredProcedure,
//    splitOn: "LoanScheduleId");

//    // var existongLoan = await multi.ReadFirstAsync<VLoans>();

//    if (!existongLoan.Any())
//    {
//        return Result.Failure<VLoans>(Error.NotFound("404", ["no data found."]));
//    }

//    //var schedules = multi.Read<VSchedule>().ToList();

//    //var paidSchedules = multi.Read<VSchedulePaid>().ToList();

//    //existongLoan.Schedules = schedules ?? [];

//    //existongLoan.PaidSchedules = paidSchedules ?? [];

//    var item = existongLoan.First();

//    return Result.Success(item);

//}

//private async Task<Result<VLoans>> GetOnlyLoanInfo(long loanId, int branchId)
//{
//    using var db = _context.Database.GetDbConnection();
//    var existongLoan = await db.QueryFirstOrDefaultAsync<VLoans>("spGetLoanOnlyDetials",
//                                new
//                                {
//                                    @LoanId = loanId,
//                                    @BranchId = branchId
//                                }, commandType: CommandType.StoredProcedure);

//    if (existongLoan is null)
//    {
//        return Result.Failure<VLoans>(Error.NotFound("404", ["no data found."]));
//    }
//    return Result.Success(existongLoan);


//}



//public async Task<Result<IEnumerable<VPaymentLines>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo)
//{

//    var db = _connectionFactory.CreateConnection();

//    var sql = @"SELECT * FROM VLoanRepaymentLines WHERE 
//                         BranchRefId=@branchId AND LoanRefId=@loanId AND 
//                         InstallmentNo=@installmentNo";
//    var paymentLines = await db.QueryAsync<VPaymentLines>(sql, new
//    {
//        branchId,
//        loanId,
//        installmentNo
//    });

//    if (!paymentLines.Any())
//    {
//        return Result.Failure<IEnumerable<VPaymentLines>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }
//    return Result.Success(paymentLines);
//}



//var items = await _context.Database
//                    .SqlQuery<VSchedule>($"EXEC  {},{loanId},{status}")
//                    .AsNoTracking()
//                    .ToListAsync();

//public async Task<Result<IEnumerable<VSchedule>>> GetTobePaidSchedules(DateTime paymentDate, int branchId, string? filter = null)
//{
//    IQueryable<VSchedule> queries = _context.Database.SqlQuery<VSchedule>($"SELECT * FROM [dbo].[fGetPaymentSchedules]({paymentDate},{branchId})")
//                                          .OrderByDescending(x => x.PaymentDate)
//                                           .ThenByDescending(x => x.FullName);
//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }

//    if (!string.IsNullOrWhiteSpace(filter))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.FullName!, $"%{filter}%") || EF.Functions.Like(p.AltName!, $"%{filter}%"));
//    }


//    var schedules = await queries.AsNoTracking().ToListAsync();


//    return Result.Success<IEnumerable<VSchedule>>(schedules);
//}

//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerLatePaymentScheduleDetails(DateTime paymentDate, int branchId, long loanId)
//{
//    using var db = _context.Database.GetDbConnection();
//    var schedules = await db.QueryAsync<VSchedule>("spGetCustomerLatePaymentScheduleDetails", new
//    {
//        paymentDate,
//        branchId,
//        loanId
//    }, commandType: CommandType.StoredProcedure);
//    if (!schedules.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }
//    return Result.Success<IEnumerable<VSchedule>>(schedules);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, ScheduleStatus status, DateTime? selectedDate, bool onScheduleOnly = false, string? filter = null)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial
//    var queries = _context.Database
//                        .SqlQuery<VSchedule>($"SELECT * FROM VSchedules WHERE BranchId={branchId}");

//    if (loanId > 0)
//    {
//        queries = queries.Where(x => x.LoanId == loanId && x.BranchId == branchId);
//    }

//    if (status == ScheduleStatus.Unpaid)
//    {
//        int?[] states = [2, 4];
//        queries = queries.Where(x => states.Contains(x.ScheduleStatusId) && x.BranchId == branchId);
//    }

//    if (status == ScheduleStatus.Paid)
//    {
//        queries = queries.Where(x => x.ScheduleStatusId == (int)ScheduleStatus.Paid && x.BranchId == branchId);
//    }

//    if (selectedDate is not null && onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate == selectedDate && x.BranchId == branchId);
//    }

//    if (selectedDate is not null && !onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate <= selectedDate && x.BranchId == branchId);
//    }

//    if (!string.IsNullOrWhiteSpace(filter))
//    {
//        queries = queries.Where(x => EF.Functions.Like(x.AltName!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.FullName!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.BranchId!.ToString(), $"%{filter}%") ||
//                                     EF.Functions.Like(x.CreditAgentNameKh!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.CreditAgentNameEn!, $"%{filter}%"));
//    }



//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    var items = await queries.AsNoTracking().ToListAsync();
//    return Result.Success<IEnumerable<VSchedule>>(items);
//}


//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, int? status, DateTime? selectedDate, bool onScheduleOnly = false)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial
//    var queries = _context.Database
//                        .SqlQuery<VSchedule>($"SELECT * FROM VSchedules WHERE BranchId={branchId} AND LoanId={loanId}");

//    if (status == 1)
//    {
//        int?[] states = [2, 4];
//        queries = queries.Where(x => states.Contains(x.ScheduleStatusId));
//    }

//    if (status == 2)
//    {
//        queries = queries.Where(x => x.ScheduleStatusId == 1);
//    }

//    if (selectedDate is not null && onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate == selectedDate);
//    }

//    if (selectedDate is not null && !onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate <= selectedDate);
//    }

//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    var items = await queries.AsNoTracking().ToListAsync();
//    return Result.Success<IEnumerable<VSchedule>>(items);
//}

//public async Task<Result<IEnumerable<VLoanRepaymentLine>>> GetCustomerPayementScheduleLines(long loanId, int installmentNo)
//{
//    using var db = _context.Database.GetDbConnection();
//    var scheduleLine = await db.QueryAsync<VLoanRepaymentLine>("spGetCustomerRepayementScheduleLines", new
//    {
//        @loanId = loanId,
//        @installmentNo = installmentNo
//    }, commandType: CommandType.StoredProcedure);
//    if (!scheduleLine.Any())
//    {
//        return Result.Failure<IEnumerable<VLoanRepaymentLine>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }
//    return Result.Success<IEnumerable<VLoanRepaymentLine>>(scheduleLine);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, DateTime? paymentDate, ScheduleStatus status = ScheduleStatus.All)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial 
//    var items = await _context.Database
//                        .SqlQuery<VSchedule>($"EXEC spGetSchedulesById {branchId},{loanId},{paymentDate},{status}")
//                        .AsNoTracking()
//                        .ToListAsync();

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    return Result.Success<IEnumerable<VSchedule>>(items);
//}


//public async Task<Result<VSchedule>> GetCustomerPaymentScheduleById(long loanId, int branchId, int installmentNo)
//{

//    using var db = _context.Database.GetDbConnection();

//    var schedule = await db.QueryFirstAsync<VSchedule>("spGetCustomerPayementSchedule",
//        new
//        {
//            @loanId = loanId,
//            @branchId = branchId,
//            @installmentNo = installmentNo
//        }, commandType: CommandType.StoredProcedure);

//    if (schedule is null)
//    {
//        return Result.Failure<VSchedule>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    return Result.Success<VSchedule>(schedule);
//}

//public async Task<Result<Pager<VLoans>>> GetLoansListAsync(int branchId, long creditAgentId, QueryParam param)
//{
//    try
//    {
//        _logger.LogInformation("Get Loan by credit agent");

//        IQueryable<VLoans> queries = _context.Database.SqlQuery<VLoans>(@$"SELECT * FROM VLoanDisbursments d WHERE d.BranchId={branchId} AND d.CreditAgentRefId={creditAgentId}");

//        if (!queries.Any())
//        {
//            return Result.Failure<Pager<VLoans>>(Error.NotFound("404", ["no data found."]));
//        }
//        if (!param.IsIncludeInActive)
//        {
//            queries = queries.Where(p => p.LoanstatusId == 4);
//        }

//        param.SortColumn = param.SortColumn!.Length == 0 ? "RegisterDate" : param.SortColumn;

//        if (param.SortDescending)
//        {
//            queries = queries.OrderByDescending(p => EF.Property<object>(p, param.SortColumn));
//        }
//        var counts = await queries.CountAsync();
//        var items = await queries.Skip(param.PageIndex)
//                                    .Take(param.PageSize)
//                                    .AsNoTracking()
//                                    .ToListAsync();
//        return Result.Success<Pager<VLoans>>(new(items, counts));
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, ex.Message);

//        return Result.Failure<Pager<VLoans>>(Error.NotFound("500", [$"{ex.Message}"]));
//    }
//}



//public async Task<Result<IEnumerable<VSchedulePaid>>> GetCustomerAlreadyPaidSchedules(long loanRefId, int branchId)
//{

//    using var db = _context.Database.GetDbConnection();

//    var paidQueries = await db.QueryAsync<VSchedulePaid>("[spGetCustomerAlreadyPaidSchedules]",
//        new
//        {
//            @loanId = loanRefId,
//            branchId = branchId
//        }, commandType: CommandType.StoredProcedure);
//    if (!paidQueries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedulePaid>>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    return Result.Success<IEnumerable<VSchedulePaid>>(paidQueries);
//}


//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long loanId, int branchId, int status)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    var items = await _context.Database
//                            .SqlQuery<VSchedule>($"EXEC spGetCustomerPaymentSchedulesByLoan {branchId},{loanId},{status}")
//                            .AsNoTracking()
//                            .ToListAsync();

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    return Result.Success<IEnumerable<VSchedule>>(items);
//}


//var guarantorIds = model.LoanGuarantors?.Select(x => x.EntId).ToList();

//var collateralLineIds = model.LoanCollateralLines?.Select(x => x.CollateralRefId).ToList();

// var guarantos = _context.OtherNames.Where(x => guarantorIds!.Contains(x.EntId)).ToList();

//var collateralLines = _context.CollateralLines.Where(x => collateralLineIds!.Contains(x.Id)).ToList();

//req!.Guarantors = guarantos;

//req!.CollateralLines = collateralLines;

//private async Task<bool> IsAddedLoanTransaction(Loan model)
//{
//    var tranx = new LoanTransaction
//    {
//        UserRefID = model.UserRefId,
//        TransactionTypeName = "Loan Disbursment",
//        TranTypeRefId = 19,
//        IsActive = true
//    };

//    var totalPrinciplePayment = model.Principle - model.AdminFeeAmount;

//    var cashMgt = new CashManagement
//    {
//        BankRefId = 11,
//        BranchRefId = model.BranchRefId,
//        Amount = totalPrinciplePayment,
//        CurrencyRefId = model.CurrencyRefId,
//        CashStatusRefId = 2,
//        Remark = "Cash goes through transaction",
//        StaffRefId = model.UserRefId,
//        Receive = "N",
//        Reject = "N",
//        TransactionDate = model.RegisterDate,
//        BusinessDate = model.RegisterDate,
//        Balance = 0
//    };

//    tranx.CashManagements!.Add(cashMgt);

//    _context.LoanTransaction.Add(tranx);

//    return await _context.SaveChangesAsync() > 0;
//}

//private async Task<bool> IsAddedLoan(Loan model)
//{
//    //var collateralLineIds = model.CollateralLines!.Select(x => x.Id).ToList();
//    //var collateralLines = _context.CollateralLines
//    //                            .Where(c => collateralLineIds.Contains(c.Id))
//    //                            .ToList();
//    //var guarantors = model.Guarantors!;
//    //var schedules = model.LoanSchedules;
//    //var disbur = ToDisbursment(model);
//    //disbur.CollateralLines!.AddRange(collateralLines);
//    //disbur.Guarantors!.AddRange(guarantors);
//    //disbur.LoanSchedules!.AddRange(schedules!);
//    _context.LoanDisbursments.Add(model);
//    return await _context.SaveChangesAsync() > 0;
//}
//private async Task<bool> IsLoanActive(long? customerId, bool isAactive)
//{
//    var updatedRow = await _context.Customers.Where(x => x.CustomerId == customerId)
//                                        .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

//    return updatedRow > 0;
//}


//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long loanRefId, int branchId, bool includePaid = false)
//{

//    using var db = _context.Database.GetDbConnection();

//    var paymentSchedules = await db.QueryAsync<VSchedule>("[spGetCustomerPaymentSchedulesByLoan]",
//        new
//        {

//        }, commandType: CommandType.StoredProcedure);



//    IQueryable<VSchedule> queries = _context.Database.SqlQuery<VSchedule>($"SELECT * FROM [dbo].[fGetPaymentSchedulesByLoan]({loanRefId},{branchId})")
//                                        .OrderBy(x => x.PaymentDate)
//                                            .ThenBy(x => x.InstallmentNo);

//    if (!includePaid)
//    {
//        queries = queries.Where(x => x.ScheduleStatusId == 2 || x.ScheduleStatusId == 4);
//    }

//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    var schedule = await queries.AsNoTracking().ToListAsync();

//    return Result.Success<IEnumerable<VSchedule>>(schedule);
//}




//private static Loan ToDisbursment(Loan model)
//{
//    return new Loan
//    {
//        LoanStatusRefId = model.LoanStatusRefId,
//        RepaymentModeRefId = model.RepaymentModeRefId,
//        LoanSubTypeRefId = model.LoanSubTypeRefId,
//        LoanCategoryRefId = model.LoanCategoryRefId,
//        BankRefId = model.BankRefId,
//        UserRefId = model.UserRefId,
//        LoanProductRefId = model.LoanProductRefId,
//        LoanAccountCode = model.LoanAccountCode,
//        Principle = model.Principle,
//        Period = model.Period,
//        PeriodName = model.PeriodName,
//        Installments = model.Installments,
//        CurrencyRefId = model.CurrencyRefId,
//        InterestRate = model.InterestRate,
//        StartDate = model.StartDate,
//        FirstPaymentDate = model.FirstPaymentDate,
//        LastDate = model.LastDate,
//        TermTypeId = model.TermTypeId,
//        InstallmentTypeId = model.InstallmentTypeId,
//        CreditAgentRefId = model.CreditAgentRefId,
//        CustomerRefId = model.CustomerRefId,
//        RegisterDate = model.RegisterDate,
//        PayEvery = model.PayEvery,
//        AdminFee = model.AdminFee,
//        AdminFeeAmount = model.AdminFeeAmount,
//        InstallmentType = model.InstallmentType,
//        CollectionFeeRate = model.CollectionFeeRate,
//        Remark = model.Remark,
//        BranchRefId = model.BranchRefId,
//        BranchName = model.BranchName,
//        Circle = model.Circle,
//        Guarantors = new List<Guarantor>(),
//        CollateralLines = new List<CollateralLine>(),
//        LoanSchedules = new List<LoanSchedule>(),
//    };
//}
//public async Task<Result<Pager<VSchedule>>> GetPaymentSchedules(DateTime paymentDate, int branchId,
//     int pageIndex = 0,
//     int pageSize = 10,
//     string? filter = null)
//{
//    IQueryable<VSchedule> queries = _context.Database
//                                                .SqlQuery<VSchedule>($"SELECT * FROM fGetloanPayments({paymentDate},{branchId})")
//                                                .OrderByDescending(x => x.PaymentDate)
//                                                    .ThenByDescending(x => x.FullName);

//    if (!string.IsNullOrWhiteSpace(filter))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.FullName, $"%{filter}%") ||
//                                    EF.Functions.Like(p.AltName, $"%{filter}%"));
//    }

//    if (!queries.Any())
//    {
//        return Result.Failure<Pager<VSchedule>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }

//    var counts = await queries.CountAsync();

//    var items = await queries
//                    .Skip(pageIndex)
//                    .Take(pageSize)
//                    .AsNoTracking()
//                    .ToListAsync();
//    return Result.Success<Pager<VSchedule>>(new(items, counts));
//}

//public async Task<MsgResponse> CreateRePayment(LoanRepayment loanRepayment, LoanTransaction transaction)
//{

//    try
//    {
//        var schedules = loanRepayment.LoanRepaymentLines;
//        _cxt.Database.BeginTransaction();
//        _cxt.LoanRepayments.Add(loanRepayment);
//        _cxt.LoanTransaction.Add(transaction);
//        await _cxt.SaveChangesAsync();
//        foreach (var item in schedules!)
//        {
//            var pp = _cxt.VSchedules.FromSql<VSchedule>($"SELECT * FROM VSchedules")
//                                   .Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                                   .FirstOrDefault();

//            if (pp!.Principal == pp.PaidPrinciple)
//            {
//                _cxt.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 1));
//            }
//            if (pp!.Principal > pp.PaidPrinciple)
//            {
//                _cxt.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 4));
//            }
//        }
//        var loanInfo = _cxt.VLoanDisbursements.FirstOrDefault(x => x.Id == loanRepayment.LoanRefId);

//        if (loanInfo is not null && loanInfo.OutStanding == 0)
//        {

//            _cxt.LoanDisbursments.Where(x => x.Id == loanRepayment.LoanRefId)
//                                    .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, 2));

//        }
//        _cxt.Database.CommitTransaction();

//        return new MsgResponse
//        {
//            IsSuccess = true,
//            Message = "Success"
//        };

//    }
//    catch (Exception ex)
//    {

//        _cxt.Database.RollbackTransaction();

//        return new MsgResponse
//        {
//            IsSuccess = false,
//            Message = ex.Message,
//        };
//    }
//public async Task<Pager<VLoans>> GetLoansAsync(int branchId, int pageIndex, int pageSize, string sortColumn, bool sortDescending = true, bool includeInactive = false, string? q = null)
//{
//    IQueryable<VLoans> queries = _context.VLoanDisbursements
//                                        .Where(x => x.BranchId == branchId);

//    if (includeInactive)
//    {
//        queries = queries.IgnoreQueryFilters();
//    }

//    if (!string.IsNullOrWhiteSpace(q))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.AltName, $"%{q}%") ||
//                                     EF.Functions.Like(p.FirstName, $"%{q}%") ||
//                                     EF.Functions.Like(p.LastName, $"%{q}%") ||
//                                     EF.Functions.Like(p.Phone, $"%{q}%"));
//    }

//    if (sortDescending)
//    {
//        queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
//    }

//    var counts = await queries.CountAsync();

//    var items = await queries.Skip(pageIndex).Take(pageSize).ToListAsync();

//    return new Pager<VLoans>(items, counts);
//}

//}


