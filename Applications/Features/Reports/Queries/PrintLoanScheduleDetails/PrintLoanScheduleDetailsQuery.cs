using Applications.Features.Reports.Dtos;
using ErrorOr;

namespace Applications.Features.Reports.Queries.PrintLoanScheduleDetails;

public record PrintLoanScheduleDetailsQuery(string? LoanId, int? BranchId) : IRequest<ErrorOr<RptScheduleDetails?>>
{
}
