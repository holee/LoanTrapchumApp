using Application.Features.Reports.Dtos;
using ErrorOr;

namespace Application.Features.Reports.Queries.PrintLoanScheduleDetails;

public record PrintLoanScheduleDetailsQuery(string? LoanId, int? BranchId) : IRequest<ErrorOr<RptScheduleDetails?>>
{
}
