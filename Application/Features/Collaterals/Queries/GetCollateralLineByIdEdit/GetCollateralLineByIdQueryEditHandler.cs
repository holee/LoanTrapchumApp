using Application.Commons.Mappers.Collaterals;
using Application.Features.Collaterals.Dtos;
using Application.Features.Collaterals.Queries.GetCollateralLineById;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralLineDetails;

public sealed class GetCollateralLineByIdQueryEditHandler : IRequestHandler<GetCollateralLineByIdEditQuery, ErrorOr<CollateralLineDtoCreate>>
{
    private readonly ICollateralService _collateralService;
    public GetCollateralLineByIdQueryEditHandler(ICollateralService collateralRepository)
    {
        _collateralService = collateralRepository;
    }
    public async Task<ErrorOr<CollateralLineDtoCreate>> Handle(GetCollateralLineByIdEditQuery request, CancellationToken cancellationToken)
    {
        var rsp = await _collateralService.GetCollateralLineByIdAsync(request.Id, request.CollateralId);

        if (rsp.IsError)
        {
            return Error.NotFound(description: $"Collateral Line with Id {request.Id} not found.");
        }

        var mappedCollateralLine = rsp.Value.MapCollateralLineEdit();

        return mappedCollateralLine;
    }
}

