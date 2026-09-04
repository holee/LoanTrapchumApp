using Application.Features.Dashbaord.Managers.Dto;
using ErrorOr;

namespace Application.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;

public record GetDbManagerLoanSummaryQuery(int? BranchId, string? UserId) : IRequest<ErrorOr<DbManagerLoanSummary>>;
