using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Applications.Services.LIsts;

public class CollateralsService(ICollateralsService collateralsService) : ICollateralsServiceImpl
{
    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? customerId)
    {

        var resp = await collateralsService.GetCustomerCollateralAsync(customerId);

        return resp;

    }

    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId)
    {

        var resp = await collateralsService.GetCustomerCollateralsAsync(customerId);

        return resp;

    }

    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed)
    {

        var resp = await collateralsService.GetCustomerCollateralsAsync(customerId, includeInUsed);

        return resp;

    }

    public async Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId)
    {
        return await collateralsService.GetCollateralByIdAsync(collateralId);
    }

    public async Task<IEnumerable<Responses.Lists.Collaterals.VCollateralLines>> GetCollateralLinesAsync(string? collateralId)
    {
        return await collateralsService.GetCollateralLinesAsync(collateralId);
    }

    public async Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param)
    {
        return await collateralsService.GetCollateralsAysnc(param);
    }

    public async Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id = 0, string? searchText = null)
    {
        return await collateralsService.GetCollateralSubTypes(id, searchText);
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId = 0, string? q = null)
    {
        return await collateralsService.GetCollateralTypes(collateralId, q);
    }

    public async Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null)
    {
        return await collateralsService.GetsCollateralAysnc(q);
    }

    public async Task<Result<string>> CreateAsync(CollateralDtoCreate entity)
    {

        var insertModel = entity.Adapt<Collateral>();

        return await collateralsService.InsertOrUpdate(insertModel);

    }

    public Task<Result<string>> UpdateAsync(CollateralDtoUpdate entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> UpdateAsync(CollateralSubType entity)
    {
        return collateralsService.InsertOrUpdateAsync(entity);
    }

}
