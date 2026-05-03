using Applications.Responses.Paginations;
using ErrorOr;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;

public record GetUserLoanSizesQuery(string? UserRefId, QueryParam Param) : IRequest<Pager<VUserLoanSizes>>;
