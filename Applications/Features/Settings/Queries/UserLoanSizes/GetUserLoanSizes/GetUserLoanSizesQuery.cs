using Applications.Features.Settings.Dto;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;

public record GetUserLoanSizesQuery(string? UserRefId, QueryParam Param) : IRequest<Pager<VUserLoanSizes>>;
