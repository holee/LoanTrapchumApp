using Application.Features.Collaterals.Queries.GetCollateralLineById;
using Application.Interfaces.Customers;
using Application.Utils;
using Domains.Models.Collaterals;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralLineDetails;
 
public sealed class GetCollateralLineByIdQueryHandler : IRequestHandler<GetCollateralLineByIdQuery, ErrorOr<CollateralLine>>
{
    private readonly ICollateralService _collateralService;
    public GetCollateralLineByIdQueryHandler(ICollateralService collateralRepository)
    {
        _collateralService = collateralRepository;
    }
    public async Task<ErrorOr<CollateralLine>> Handle(GetCollateralLineByIdQuery request, CancellationToken cancellationToken)
    {
        return await _collateralService.GetCollateralLineByIdAsync(request.Id, request.CollateralId);
    }
}

