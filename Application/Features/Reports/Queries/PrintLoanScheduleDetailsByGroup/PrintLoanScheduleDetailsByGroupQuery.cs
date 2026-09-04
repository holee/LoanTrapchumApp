using Application.Features.Reports.Dtos;
using ErrorOr;

namespace Application.Features.Reports.Queries.PrintLoanScheduleDetailsByGroup;

public record PrintLoanScheduleDetailsByGroupQuery(string? LoanId, int? BranchId,int? ScheduleGroup) : IRequest<ErrorOr<RptScheduleDetails?>>
{
}
