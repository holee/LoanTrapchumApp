using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses.ViewModels.Reports;

namespace Applications.Interfaces.Loans
{
    public interface ILoanReportService
    {
        Task<DisbursmentsRpt?> GetLoanScheduleAsync(int? branchId, string? loanId);
        Task<VLoanDisbursments?> GetLoanWithTopSchedulesAsync(string? loanId, int? branchId, CancellationToken cancellation = default);
        Task<VLoanDisbursments?> PrintLoanScheduleAsync(int? branchId, string? loanId);
    }

}
