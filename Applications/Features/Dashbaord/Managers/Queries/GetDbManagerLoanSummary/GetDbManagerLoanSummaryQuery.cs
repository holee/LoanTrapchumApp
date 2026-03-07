using Applications.Features.Dashbaord.Managers.Dto;
using ErrorOr;

namespace Applications.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;

public record GetDbManagerLoanSummaryQuery(int? BranchId, string? UserId) : IRequest<ErrorOr<DbManagerLoanSummary>>;
