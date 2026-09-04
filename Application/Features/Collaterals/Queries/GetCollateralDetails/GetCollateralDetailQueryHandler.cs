using Application.Features.Collaterals.Dtos;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralDetails;

public sealed class GetCollateralDetailQueryHandler : IRequestHandler<GetCollateralDetailQuery, ErrorOr<VCustomerCollaterals>>
{

    private readonly ICollateralService _cusCollateralService;

    public GetCollateralDetailQueryHandler(ICollateralService cusCollateralService)
    {
        _cusCollateralService = cusCollateralService;
    }

    public async Task<ErrorOr<VCustomerCollaterals>> Handle(GetCollateralDetailQuery request, CancellationToken cancellationToken = default)
    {
        return await _cusCollateralService.GetCollateralDetailAsync(request.CustomerId, request.Include, cancellationToken);
    }
}


//public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? collateralId, bool include = false)
//{

//    if (string.IsNullOrWhiteSpace(collateralId)) 
//    {
//        return Result.Failure<VCustomerCollaterals>(ErrorResponse.Validation("Collateral ID Is Required."));
//    }

//    return await _cusCollateralService.GetCollateralsAsync(collateralId, include); 

//}




//public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? collateralId)
//{

//    if (string.IsNullOrWhiteSpace(collateralId))
//    {
//        return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.Validation("Collateral ID is required"));
//    }

//    return await _cusCollateralService.GetCollateralLinesAsync(collateralId);

//}


//public async Task<Result<VCollateralLines>> GetCustomerCollateralLine(long? id, string? collateralId)
//{

//    if (string.IsNullOrWhiteSpace(collateralId))
//    {
//        return Result.Failure<VCollateralLines>(ErrorResponse.Validation("Collateral ID is required"));
//    }

//    return await _cusCollateralService.GetCollateralLineDetailAsync(id, collateralId);

//}


//public async Task<Result<string>> UpsertCollateralLine(CollateralLineDtoCreate req)
//{

//    if (req.Id == 0 || string.IsNullOrWhiteSpace(req.CollateralRefId))
//    {
//        return Result.Failure<string>(ErrorResponse.Validation("Collateral Line ID and Collateral Reference ID are required"));
//    }

//    var model = req.MapCollateralLine();

//    return await _cusCollateralService.UpsertCollateralLineAsync(model);
//}

 

