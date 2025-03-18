using Applications.Enumerations;
using Applications.Requests.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public interface ILoanService
    {
        Task<Result<string>> CreateLoanAsync(LoanDtoCreate disbursement);
        Task<Result<string>> DeleteAsync(long loanId,int branchId);
        Task<Result<string>> UpdateAsync(Loan disbursement);
        Task<Result<string>> ChangedCreditAgent(int branchId, LoanChangedCaDtoCreate req);
        Task<Result<string>> ChangedCreditAgents(int branchId, List<LoanChangedCaDtoCreate> req);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(long loanId, int branchId, ScheduleStatus status);
        Task<Pager<VLoanDisbursments>> GetLoansAsync(int branchId, long creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
        Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All);


        //Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = false, string? filter = null);
        //Task<Result<Loan>> GetLoanByIdAsync(long loanId, int branchId, params string[] includes);
        //Task<Result<IEnumerable<VPaymentLines>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo);
        //Task<Result<IEnumerable<VSchedule>>> GetLatePaymentSchedules(int? branchId, DateTime? paymentDate = null, string? filter = null);
        //Task<Result<VSchedule>> GetPaymentScheduleById(long loanId, long  loanScheduleId, int branchId, bool related);
        //Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, ScheduleStatus status, DateTime? selectedDate, bool onScheduleOnly = false, string? filter = null);

    }

}