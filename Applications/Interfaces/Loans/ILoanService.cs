using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;
using ErrorOr;

namespace Applications.Interfaces.Loans
{


    public interface ILoanService
    {
        Task<Result<bool>> CreateLoanAsync(LoanDisbursment req, CancellationToken cancellation = default);
        Task<ErrorOr.ErrorOr<DeleteResult>> DeleteAsync(string? loanId,int? branchId, CancellationToken cancellation = default);
        Task<Result<string>> UpdateAsync(LoanDisbursment disbursement);
        Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req);
        Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCa> req);
        Task<ErrorOr.ErrorOr<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false);
       
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
        Task<ErrorOr.ErrorOr<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default);
        Task<VLoanDisbursments?> GetLoanByCreditAgentIdAsync(string? creditAgentId, string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default);
        Task<IEnumerable<VSchedule>> GetLoanSchedulesAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, QueryParam? param = null, CancellationToken cancellation = default);
        Task<(Pager<VLoanDisbursments>, int Count)> GetLoansByBranchAsync(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
        Task<IEnumerable<VLoanDisbursementCollateral>> GetRelatedCollaterals(string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<IEnumerable<VLoanDisbursementGuarantors>> GetRelatedGaurantors(string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<IEnumerable<VLoanDisbursementGuarantors>> GetRelatedGaurantors(string? loanId, int? branchId, CancellationToken cancellation = default, params string[] columns);
        Task<IEnumerable<VLoanDisbursementDocuments>> GetRelatedDocuments(string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<ErrorOr<DisburseResult>> CreateAsync(LoanDisbursment req, CancellationToken cancellation = default);
        Task<ErrorOr<string>> CloseLoanDisbursementAsync(int? branchId, string? loanId, string? customerId, CancellationToken cancellation = default);
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, DateTime? start = null, DateTime? to = null, QueryParam? param = null, CancellationToken cancellation = default);
        Task<Pager<VSchedule>> GetDueScheduleLoanByIdAsync(string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<Pager<VSchedule>> GetScheduleLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus state, CancellationToken cancellation = default);
    }

}
