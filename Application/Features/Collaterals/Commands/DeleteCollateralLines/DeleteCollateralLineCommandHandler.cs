using Application.Bases;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Commands.DeleteCollateralLines;

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

