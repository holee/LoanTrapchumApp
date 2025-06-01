using Applications.Enumerations;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanReportService 
    {
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false);
        Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
        Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId);
    }

}
