using Application.Features.Collaterals.Dtos;
using Application.Responses.Paginations;
using Application.Utils;

namespace Application.Features.Collaterals.Queries.GetCollateralAsPaging;

public record GetCollateralAsPagingQuery(QueryParam Param) : IRequest<Pager<VCollateral>>;
