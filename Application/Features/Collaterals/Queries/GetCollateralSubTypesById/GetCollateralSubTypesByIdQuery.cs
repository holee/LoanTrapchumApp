namespace Application.Features.Collaterals.Queries.GetCollateralSubTypesById;

public record GetCollateralSubTypesByIdQuery(int? CollateralTypeId, string? searchTerm)
    :IRequest<IEnumerable<CollateralSubTypeDto>>;
