using Application.Features.Reports.Dtos;

namespace Application.Interfaces;

public interface IPrintingLoanService
{
    Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, CancellationToken cancellationToken = default);
    Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, int? scheduleGroup, CancellationToken cancellationToken = default);
}
