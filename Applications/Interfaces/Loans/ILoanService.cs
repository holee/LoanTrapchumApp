using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{


    public interface ILoanService
    {
        Task<Result<bool>> CreateLoanAsync(LoanDisbursment req);
        Task<Result<string>> DeleteAsync(string? loanId,int? branchId);
        Task<Result<string>> UpdateAsync(LoanDisbursment disbursement);
        Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req);
        Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCa> req);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false);
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId);
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default);
        Task<VLoanDisbursments?> GetLoanByCreditAgentIdAsync(string? creditAgentId, string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default);
        Task<IEnumerable<VSchedule>> GetLoanSchedulesAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, QueryParam? param = null, CancellationToken cancellation = default);
        Task<(Pager<VLoanDisbursments>, int Count)> GetLoansByBranchAsync(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
    }

}
