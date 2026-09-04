using Application.Features.Collaterals.Dtos;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralByCustomer;

public sealed class  GetCollateralByCustomerQueryHandler : IRequestHandler<GetCollateralByCustomerQuery, ErrorOr<VCustomerCollaterals>>
{
    
    private readonly ICollateralService _collateralService;

    public GetCollateralByCustomerQueryHandler(ICollateralService collateralService)
    {
        _collateralService = collateralService;
    }

    public async Task<ErrorOr<VCustomerCollaterals>> Handle(GetCollateralByCustomerQuery request, CancellationToken cancellationToken = default)
    {
        return await _collateralService.GetCollateralDetailAsync(request.CustomerId, request.Include, cancellationToken);
    }
}