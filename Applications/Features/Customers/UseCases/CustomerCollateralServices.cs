using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Mappers;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;

namespace Applications.Features.Customers.UseCases;

public sealed class CustomerCollateralServices
{

    private readonly ICustomerCollateralService _cusCollateralService;

    public CustomerCollateralServices(ICustomerCollateralService cusCollateralService)
    {
        _cusCollateralService = cusCollateralService;
    }


    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? collateralId, bool include = false)
    {

        if (string.IsNullOrWhiteSpace(collateralId)) 
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.Validation("Collateral ID Is Required."));
        }

        return await _cusCollateralService.GetCustomerCollateralsAsync(collateralId, include); 

    }




    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? collateralId)
    {

        if (string.IsNullOrWhiteSpace(collateralId))
        {
            return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.Validation("Collateral ID is required"));
        }

        return await _cusCollateralService.GetCustomerCollateralLinesAsync(collateralId);

    }


    public async Task<Result<VCollateralLines>> GetCustomerCollateralLine(long? id, string? collateralId)
    {

        if (string.IsNullOrWhiteSpace(collateralId))
        {
            return Result.Failure<VCollateralLines>(ErrorResponse.Validation("Collateral ID is required"));
        }

        return await _cusCollateralService.GetCustomerCollateralLineAsync(id, collateralId);

    }


    public async Task<Result<string>> UpsertCollateralLine(CollateralLineDtoCreate req)
    {

        if (req.Id == 0 || string.IsNullOrWhiteSpace(req.CollateralRefId))
        {
            return Result.Failure<string>(ErrorResponse.Validation("Collateral Line ID and Collateral Reference ID are required"));
        }

        var model = req.MapCollateralLine();

        return await _cusCollateralService.UpsertCollateralLineAsync(model);
    }


    //public async Task<Result<string>> DeleteCollateralLine(int? id, string? collateralId)
    //{

    //    if (!id.HasValue || string.IsNullOrWhiteSpace(collateralId))
    //    {
    //        return Result.Failure<string>(ErrorResponse.Validation("Collateral Line ID and Collateral Reference ID are required"));
    //    }

    //    return await _cusCollateralService.DeleteCollateralLineAsync(id, collateralId);
    //}



}
