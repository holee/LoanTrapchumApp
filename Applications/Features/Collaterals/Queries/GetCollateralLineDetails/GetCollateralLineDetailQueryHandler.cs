using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Queries.GetCollateralLineDetails;

public sealed class GetCollateralLineDetailQueryHandler : IRequestHandler<GetCollateralLineDetailQuery, ErrorOr<VCollateralLines>>
{
    private readonly ICollateralService _collateralService;
    public GetCollateralLineDetailQueryHandler(ICollateralService collateralRepository)
    {
        _collateralService = collateralRepository;
    }
    public async Task<ErrorOr<VCollateralLines>> Handle(GetCollateralLineDetailQuery request, CancellationToken cancellationToken)
    {
        return await _collateralService.GetCollateralLineDetailAsync(request.Id, request.CollateralId,cancellationToken);
        
    }
}
