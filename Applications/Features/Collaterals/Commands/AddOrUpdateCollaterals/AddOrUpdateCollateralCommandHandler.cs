using Applications.Commons.Mappers.Collaterals;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;

public sealed class AddOrUpdateCollateralCommandHandler : IRequestHandler<AddOrUpdateCollateralCommand, ErrorOr<string>>
{
    private readonly ICollateralService _cusCollateralService;
    public AddOrUpdateCollateralCommandHandler(ICollateralService cusCollateralService)
    {
        _cusCollateralService = cusCollateralService;
    }
    public async Task<ErrorOr<string>> Handle(AddOrUpdateCollateralCommand request, CancellationToken cancellationToken = default)
    {

        var model = request.Request.MapCollateral();

        return await _cusCollateralService.InsertOrUpdate(model, request.Detaching, cancellationToken);
    }
}
