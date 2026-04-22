using Applications.Features.Settings.Dto;
using Applications.Responses.Paginations;

namespace Applications.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;

public record GetCollateralSubTypesQuery(QueryParam Param) : IRequest<Pager<CollateralSubTypeDto>>;
