using Applications.Features.Collaterals.Dtos;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Collaterals.Queries.GetCollateralLinesAsPaging;

public record GetCollateralLineAsPagingQuery(string? CustomerId, QueryParam Query) : IRequest<Pager<VCollateralLines>>;
