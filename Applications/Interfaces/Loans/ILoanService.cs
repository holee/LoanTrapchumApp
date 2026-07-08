using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Loans.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;
using ErrorOr;

namespace Applications.Interfaces.Loans;



public interface ILoanService
{
    Task<ErrorOr<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default);
    Task<VLoanDisbursments?> GetLoanByCreditAgentIdAsync(string? creditAgentId, string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default);
    Task<IEnumerable<VSchedule>> GetLoanSchedulesAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, QueryParam? param = null, CancellationToken cancellation = default);
    Task<(Pager<VLoanDisbursments>, int Count)> GetLoansByBranchAsync(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
    Task<IEnumerable<VLoanDisbursementCollateral>> GetRelatedCollaterals(string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<IEnumerable<VLoanDisbursementGuarantors>> GetRelatedGaurantors(string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<IEnumerable<VLoanDisbursementGuarantors>> GetRelatedGaurantors(string? loanId, int? branchId, CancellationToken cancellation = default, params string[] columns);
    Task<IEnumerable<VLoanDisbursementDocuments>> GetRelatedDocuments(string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<ErrorOr<DisburseResult>> CreateLoanDisbursementAsync(LoanDisbursment req, CancellationToken cancellation = default);
    Task<ErrorOr<string>> CloseLoanDisbursementAsync(int? branchId, string? loanId, string? customerId, CancellationToken cancellation = default);
    Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, DateTime? start = null, DateTime? to = null, QueryParam? param = null, CancellationToken cancellation = default);
    Task<Pager<VSchedule>> GetDueScheduleLoanByIdAsync(string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<Pager<VSchedule>> GetScheduleLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus state, DateTime? dueDate = null, CancellationToken cancellation = default);
    Task<Pager<VLoanCollateral>> GetLoanCollaterals(string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<bool> WidthrawalLoanCollateralsAsync(IEnumerable<WidthrawCollateralDtoCreate> req, CancellationToken cancellation = default);
    Task<LoanDtoCreate> GetLoanForEditAsync(string? loanId, int? branchId, CancellationToken cancellationToken = default);
    Task<DisburseResult> EditLoanDisbursementAsync(LoanDisbursment req, CancellationToken cancellation = default);
    Task<DeleteResult> DeleteLoanDisbursementAsync(string? loanId, int? branchId, string? customerId, CancellationToken cancellation = default);
    Task<LoanDisbursment?> GetLoanByIdAsync(string? loanId, int? branchId, CancellationToken cancellation = default);
    Task<DeleteResult> RequestDeleteLoanAsync(RequestDeleteDtoCreate request, CancellationToken cancellation = default);
    Task<DisburseResult> ApproveDeleteRequestAsync(RequestDeleteDtoCreate request, CancellationToken cancellation = default);
    Task<DeleteResult> DeniedDeleteRequestAsync(RequestDeleteDtoCreate request, CancellationToken cancellation = default);
    Task<AppResult> RequestEditLoanAsync(RequestDeleteDtoCreate request, CancellationToken cancellation = default);
    Task<AppResult> ApproveEditLoanRequestAsync(RequestDeleteDtoCreate request, CancellationToken cancellation = default);
}
