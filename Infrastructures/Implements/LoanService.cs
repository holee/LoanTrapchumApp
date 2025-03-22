using System.Data;
using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.Disbursements;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class LoanService : ILoanService
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

        public Task<Stream> DownLoadScheduleAsExcel(Guid? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownLoadScheduleAsPdf(Guid? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<VLoanDisbursments>> GetLoanByIdAsync(Guid loanId, int branchId, ScheduleStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> DeleteAsync(Guid? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCaDtoCreate> req)
        {
            throw new NotImplementedException();
        }

        public Task<Result<VLoanDisbursments>> GetLoanByIdAsync(Guid? loanId, int? branchId, ScheduleStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<VLoanDisbursments>> GetLoansAsync(int? branchId, Guid? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(Guid? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
        {
            throw new NotImplementedException();
        }
    }
}
