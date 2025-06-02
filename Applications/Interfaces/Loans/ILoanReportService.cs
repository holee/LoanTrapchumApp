using Applications.Responses.ViewModels.Reports;

namespace Applications.Interfaces.Loans
{
    public interface ILoanReportService
    {
        Task<DisbursmentsRpt?> GetLoanScheduleAsync(int? branchId, string? loanId);
    }

}
