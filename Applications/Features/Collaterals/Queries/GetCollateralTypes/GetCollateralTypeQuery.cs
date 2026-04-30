namespace Applications.Features.Collaterals.Queries.GetCollateralTypes;

public record GetCollateralTypeQuery(int? typeId, string? typeName) : IRequest<IEnumerable<CollateralTypeDto>>;

