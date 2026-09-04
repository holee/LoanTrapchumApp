using Application.Features.Collaterals.Dtos;
using Application.Responses.Paginations;
using Application.Utils;

namespace Application.Features.Collaterals.Queries.GetCollateralLinesAsPaging;

public record GetCollateralLineAsPagingQuery(string? CustomerId, QueryParam Query) : IRequest<Pager<VCollateralLines>>;
