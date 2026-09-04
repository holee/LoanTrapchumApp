using Application.Responses.Paginations;
using ErrorOr;

namespace Application.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;

public record GetUserLoanSizesQuery(string? UserRefId, QueryParam Param) : IRequest<Pager<VUserLoanSizes>>;
