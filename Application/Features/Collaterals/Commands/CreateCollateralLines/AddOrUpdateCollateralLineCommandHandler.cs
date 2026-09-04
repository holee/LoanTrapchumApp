using Application.Commons.Mappers.Collaterals;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Commands.CreateCollateralLines;

public sealed class AddOrUpdateCollateralLineCommandHandler : IRequestHandler<AddOrUpdateCollateralLineCommand, ErrorOr<string>>
{

    private readonly ICollateralService _collateralService;

    public AddOrUpdateCollateralLineCommandHandler(ICollateralService collateralService)
    {
        _collateralService = collateralService;
    }

    public async Task<ErrorOr<string>> Handle(AddOrUpdateCollateralLineCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.Request.MapCollateralLine();

        return await _collateralService.InsertOrUpdateCollateralLineAsync(model, cancellationToken);
    }
}