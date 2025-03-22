using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.Disbursements;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public interface ILoanServiceImpl
    {
        Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req);
        Task<Result<string>> ChangedCreditAgent(int? branchId, List<LoanChangedCaDtoCreate> req);
        Task<Result<string>> CreateLoanAsync(LoanDtoCreate disbursement);
        Task<Result<string>> DeleteAsync(long loanId, int branchId);
        Task<Stream> DownLoadScheduleAsExcel(Guid? loanId, int? branchId);
        Task<Stream> DownLoadScheduleAsPdf(Guid? loanId, int? branchId);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(Guid loanId, int branchId, ScheduleStatus status);
        Task<Pager<VLoanDisbursments>> GetLoansAsync(int? branchId, Guid? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
        Task<Pager<VLoanDisbursments>> GetLoansAsync(int branchId, Guid? customerRequestId, Guid loanRequestId, bool v);
        Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All);
        Task<Result<string>> UpdateAsync(Loan disbursement);
    }
    public sealed class LoanService : ILoanServiceImpl
    {
        public Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangedCreditAgent(int? branchId, List<LoanChangedCaDtoCreate> req)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> CreateLoanAsync(LoanDtoCreate disbursement)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> DeleteAsync(long loanId, int branchId)
        {
            throw new NotImplementedException();
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

        public Task<Pager<VLoanDisbursments>> GetLoansAsync(int? branchId, Guid? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<VLoanDisbursments>> GetLoansAsync(int branchId, Guid? customerRequestId, Guid loanRequestId, bool v)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> UpdateAsync(Loan disbursement)
        {
            throw new NotImplementedException();
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
