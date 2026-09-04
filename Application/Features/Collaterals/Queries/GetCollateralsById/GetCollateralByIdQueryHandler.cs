using Application.Interfaces.Customers;
using Application.Utils;
using Domains.Models.Collaterals;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralsById;

public sealed class GetCollateralByIdQueryHandler : IRequestHandler<GetCollateralByIdQuery, ErrorOr<Collateral>>
{
    private readonly ICollateralService _collateralsService;
    public GetCollateralByIdQueryHandler(ICollateralService collateralsService)
    {
        _collateralsService = collateralsService;
    }
    public async Task<ErrorOr<Collateral>> Handle(GetCollateralByIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _collateralsService.GetCollateralByIdAsync(request.CollateralId, cancellationToken);
    }
}
