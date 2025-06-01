using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.Disbursements;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Schedules;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Mapster;
using QuestPDF.Fluent;

namespace Applications.Services.Loans
{
    public sealed class LoanService(ILoanService loanService) : ILoanServiceImpl
    {
        public Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangedCreditAgent(int? branchId, List<LoanChangedCaDtoCreate> req)
        {
            throw new NotImplementedException();
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
            
            var model = req.MapLoan();
            
            model.LoanTransaction = tranx;
            
            return await loanService.CreateLoanAsync(model);
        }

        

        public Task<Result<string>> UpdateAsync(LoanDisbursment disbursement)
        {
            throw new NotImplementedException();
        }
        public Task<Result<string>> DeleteAsync(string? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownLoadScheduleAsExcel(string? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Stream>> DownLoadScheduleAsPdf(string? loanId, int? branchId)
        {

            var rsp = await loanService.GetLoanByIdAsync(loanId, branchId, ScheduleStatus.All);

            if (rsp.IsSuccess)
            {

                var item = rsp.Data.Adapt<LoanScheduleVm>();

                var pdf = new SchedulePdfService(item);

                var stream = new MemoryStream(pdf.GeneratePdf());

                return Result.Success<Stream>(stream);

            }

            return Result.Failure<Stream>(ErrorResponse.NotFound(""));
        }

        public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status,bool includeSchedule=false)
        {
            return await loanService.GetLoanByIdAsync(loanId, branchId, status,includeSchedule);
        }

        public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId)
        {
            return await loanService.GetLoanByIdAsync(loanId, branchId);
        }

        public async Task<Pager<VLoanDisbursments>> GetLoansAsync(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
        {
            return await loanService.GetLoansByBranchId(branchId, userId, status, param);
        }

        public async Task<Result<byte[]>> ExportScheduleAsPdf(string? loanId, int? branchId)
        {
            var rsp = await loanService.GetLoanByIdAsync(loanId, branchId, ScheduleStatus.All);

            if (!rsp.IsSuccess)
            {
                return Result.Failure<byte[]>(ErrorResponse.NotFound("No Schedule to print"));
            }
            var item = rsp.Data.Adapt<LoanScheduleVm>();

            var pdf = new SchedulePdfService(item);

            return Result.Success(pdf.GeneratePdf());
        }
    }

}


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
