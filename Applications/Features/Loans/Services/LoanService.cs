using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Loans.Command.CreateLoan;
using Applications.Features.Loans.Mappers;
using Applications.Features.Loans.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Mapster;

namespace Applications.Features.Loans.Services
{
    public sealed class LoanService(ILoanService loanService, ICreditAgentService creditAgentService) : ILoanServiceImpl
    {
        public async Task<Result<string>> ChangedCreditAgent(int? branchId, List<LoanChangedCaDtoCreate> req)
        {
            if (!branchId.HasValue)
            {
                return Result.Failure<string>(ErrorResponse.Validation("BranchId and Credit Agent details are required."));
            }

            var model = req.Adapt<List<LoanChangedCa>>();

            return await creditAgentService.ChangedCreditAgents(branchId, model);
        }
        
        public async Task<Result<bool>> CreateLoanAsync(LoanDtoCreate req)
        {

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

            var model = req.MapLoanCreate();

            model.LoanTransaction = tranx;

            return await loanService.CreateLoanAsync(model);
        }
        public async Task<DisburseResult> CreateAsync(LoanDtoCreate req)
        {

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

            var model = req.MapLoanCreate();

            model.LoanTransaction = tranx;

            var command=new CreateLoanCommand(req);


            var rsp = await  loanService.CreateAsync(model);

            return rsp.Value;


            //return rsp.IsSuccess ? new DisburseResult(true, "New Loan Was Disbursed") : new DisburseResult();
        }
        
        public Task<Result<string>> UpdateAsync(LoanDisbursment disbursement)
        {
            throw new NotImplementedException();
        }
        public Task<Result<string>> DeleteAsync(string? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }

        //public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default)
        //{
        //    if (string.IsNullOrEmpty(loanId) || !branchId.HasValue)
        //    {
        //        return Result.Failure<VLoanDisbursments>(ErrorResponse.Validation("LoanId and BranchId must be provided."));
        //    }

        //    return await loanService.GetLoanByIdAsync(loanId, branchId, status, include, cancellation);
        //}

        public async Task<Pager<VLoanDisbursments>> GetLoansByBranchIdAsync(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default)
        {
            return await loanService.GetLoansByBranchId(branchId, userId, status, param, cancellation);
        }

        public async Task<Result<VLoanDisbursments>> GetLoanByCreditAgentIdAsync(string? creditAgentId, string? loanId, int? branchId, CancellationToken cancellation = default)
        {
            if (string.IsNullOrEmpty(creditAgentId) || string.IsNullOrEmpty(loanId) || !branchId.HasValue)
            {
                return Result.Failure<VLoanDisbursments>(ErrorResponse.Validation($"{creditAgentId} OR {loanId} OR {branchId} is required,"));
            }


            var item = await loanService.GetLoanByCreditAgentIdAsync(creditAgentId, loanId, branchId, cancellation);

            if (item == null)
            {
                return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("Loan Not Found."));
            }

            return Result.Success(item);

        }

        public async Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentsAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default)
        {
            if (!branchId.HasValue)
                throw new ArgumentNullException(nameof(branchId), "Branch ID must not be null.");

            return await loanService.GetLoanByCreditAgentAsync(creditAgentId, branchId, cancellation);
        }

       
    }

}

//public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status,bool include=false)
//{

//    if (string.IsNullOrEmpty(loanId) || !branchId.HasValue)
//    {
//        return Result.Failure<VLoanDisbursments>(ErrorResponse.Validation("LoanId and BranchId must be provided."));
//    }

//    return await loanService.GetLoanByIdAsync(loanId, branchId, status,include);
//}

//public async Task<Pager<VLoanDisbursments>> GetLoansByBranchIdAsync(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
//{
//    return await loanService.GetLoansByBranchId(branchId, userId, status, param);
//}

//public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId)
//{
//    return await loanService.GetLoanByIdAsync(loanId, branchId);
//}

//Task<Result<string>> CreateLoanAsync(LoanDtoCreate disbursement);
//Task<Result<string>> DeleteAsync(long loanId,int branchId);
//Task<Result<string>> UpdateAsync(Loan disbursement);
//Task<Result<string>> ChangedCreditAgent(int branchId, LoanChangedCaDtoCreate req);
//Task<Result<string>> ChangedCreditAgents(int branchId, List<LoanChangedCaDtoCreate> req);
//Task<Result<VLoanDisbursments>> GetLoanByIdAsync(long loanId, int branchId, ScheduleStatus status);
//Task<Pager<VLoanDisbursments>> GetLoansAsync(int branchId, long creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
//Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All);


//Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = false, string? filter = null);
//Task<Result<Loan>> GetLoanByIdAsync(long loanId, int branchId, params string[] includes);
//Task<Result<IEnumerable<VPaymentLines>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo);
//Task<Result<IEnumerable<VSchedule>>> GetLatePaymentSchedules(int? branchId, DateTime? paymentDate = null, string? filter = null);
//Task<Result<VSchedule>> GetPaymentScheduleById(long loanId, long  loanScheduleId, int branchId, bool related);
//Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, ScheduleStatus status, DateTime? selectedDate, bool onScheduleOnly = false, string? filter = null);
