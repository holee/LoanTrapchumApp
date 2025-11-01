using Applications.Bases;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Commands.DeleteCollateralLines;

public sealed class DeleteCollateralLineCommandHandler : IRequestHandler<DeleteCollateralLineCommand, ErrorOr<DeleteResult>>
{
    private readonly ICollateralService _cusCollateralService;
    public DeleteCollateralLineCommandHandler(ICollateralService cusCollateralService)
    {
        _cusCollateralService = cusCollateralService;
    }
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteCollateralLineCommand request, CancellationToken cancellationToken = default)
    {
        return await _cusCollateralService.DeleteCollateralLineAsync(request.Id, request.CollateralId, cancellationToken);
    }
}

