using Applications.Enumerations;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.Disbursements;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{


    public interface ILoanService
    {
        Task<Result<string>> CreateLoanAsync(LoanDtoCreate disbursement);
        Task<Result<bool>> CreateLoanAsync(LoanDisbursment req);
        Task<Result<string>> DeleteAsync(string? loanId,int? branchId);
        Task<Result<string>> UpdateAsync(LoanDisbursment disbursement);
        Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req);
        Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCaDtoCreate> req);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false);
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId);
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default);
    }

}
