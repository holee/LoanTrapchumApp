using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Loans.Requests;
using Applications.Features.Loans.Responses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Loans;

namespace Applications.Features.Loans.Services;

public interface ILoanServiceImpl
{
    Task<Result<string>> ChangedCreditAgent(int? branchId, List<LoanChangedCaDtoCreate> req);
    Task<Result<bool>> CreateLoanAsync(LoanDtoCreate req);
    Task<Result<string>> DeleteAsync(string? loanId, int? branchId);
    Task<Result<string>> UpdateAsync(LoanDisbursment disbursement);  
    Task<Pager<VLoanDisbursments>> GetLoansByBranchIdAsync(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
    Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default);
    Task<Result<VLoanDisbursments>> GetLoanByCreditAgentIdAsync(string? creditAgentId,string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentsAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default);
    Task<DisburseResult> CreateAsync(LoanDtoCreate req);
}



//Task<Pager<VLoanDisbursments>> GetLoansByBranchIdAsync(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);  

//Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status,bool include=false);
//Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId);
//Task<Stream> DownLoadScheduleAsExcel(string? loanId, int? branchId);
//Task<Result<Stream>> DownLoadScheduleAsPdf(string? loanId, int? branchId);
//Task<Result<byte[]>> ExportScheduleAsPdf(string? loanId, int? branchId);
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
