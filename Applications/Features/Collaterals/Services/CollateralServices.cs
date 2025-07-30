using Applications.Commons.Mappers;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;

namespace Applications.Features.Collaterals.Services;

public sealed class CollateralServices
{
    private readonly ICollateralsService collateralsService;

    public CollateralServices(ICollateralsService collateralsService)
    {
        this.collateralsService = collateralsService;
    }

    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? collateralId)
    {

        if (string.IsNullOrWhiteSpace(collateralId))
        {
            return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.Validation("Collateral ID is required"));
        }

        return await collateralsService.GetCustomerCollateralLinesAsync(collateralId);

    }


    public async Task<Result<VCollateralLines>> GetCustomerCollateralLine(long? id, string? collateralId)
    {

        if (string.IsNullOrWhiteSpace(collateralId)) 
        {
            return Result.Failure<VCollateralLines>(ErrorResponse.Validation("Collateral ID is required"));
        }

        return await collateralsService.GetCustomerCollateralLineAsync(id, collateralId);

    }


    //public async Task<Result<VCollateralLines>> UpsertCollateralLine(CollateralLineDtoUpdate req) 
    //{

    //    var model = req.MapCollateralLine();


         

    //}





}
