using Applications.Features.Collaterals.Dtos;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Collaterals.Queries.GetCollateralAsPaging;

public record GetCollateralAsPagingQuery(QueryParam Param) : IRequest<Pager<VCollateral>>;
