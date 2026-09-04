namespace Application.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;

public record GetCollateralSubTypesQuery(QueryParam Param) : IRequest<Pager<CollateralSubTypeDto>>;
