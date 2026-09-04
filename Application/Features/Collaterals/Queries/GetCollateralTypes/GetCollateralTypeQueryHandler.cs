using Application.Interfaces.Lists;

namespace Application.Features.Collaterals.Queries.GetCollateralTypes;

public sealed class GetCollateralTypeQueryHandler : IRequestHandler<GetCollateralTypeQuery, IEnumerable<CollateralTypeDto>>
{
    private readonly ICollateralsService _service;
    public GetCollateralTypeQueryHandler(ICollateralsService service)
    {
        _service = service;
    }
    public async Task<IEnumerable<CollateralTypeDto>> Handle(GetCollateralTypeQuery request, CancellationToken cancellationToken)
       => await _service.GetCollateralTypes(request.typeId, request.typeName);
}

