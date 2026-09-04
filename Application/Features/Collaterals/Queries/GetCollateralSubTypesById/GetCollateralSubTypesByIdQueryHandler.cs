using Application.Interfaces.Lists;

namespace Application.Features.Collaterals.Queries.GetCollateralSubTypesById;

public sealed class GetCollateralSubTypesByIdQueryHandler : IRequestHandler<GetCollateralSubTypesByIdQuery, IEnumerable<CollateralSubTypeDto>>
{
    private readonly ICollateralsService _service; 
    public GetCollateralSubTypesByIdQueryHandler(ICollateralsService service)
    {
        _service = service;
    }

    public async Task<IEnumerable<CollateralSubTypeDto>> Handle(GetCollateralSubTypesByIdQuery request, CancellationToken cancellationToken)
        => await _service.GetCollateralSubTypes(request.CollateralTypeId, request.searchTerm, cancellationToken);
}