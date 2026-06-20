using Applications.Features.Reports.Dtos;
using ErrorOr;

namespace Applications.Features.Reports.Queries.PrintLoanScheduleDetailsByGroup;

public record PrintLoanScheduleDetailsByGroupQuery(string? LoanId, int? BranchId,int? ScheduleGroup) : IRequest<ErrorOr<RptScheduleDetails?>>
{
}
