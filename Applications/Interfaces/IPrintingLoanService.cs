using Applications.Features.Reports.Dtos;

namespace Applications.Interfaces;

public interface IPrintingLoanService
{
    Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, CancellationToken cancellationToken = default);
    Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, int? scheduleGroup, CancellationToken cancellationToken = default);
}
