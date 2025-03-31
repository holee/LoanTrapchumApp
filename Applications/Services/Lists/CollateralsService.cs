using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;
using Mapster;

namespace Applications.Services.LIsts;

public class CollateralsService(ICollateralsService collateralsService) : ICollateralsServiceImpl
{
    public async Task<Result<CollateralResponse>> GetCollateralByCustomerAsync(Guid? customerId)
    {
      
        var resp = await collateralsService.GetCollateralByCustomerAsync(customerId);

        return resp;

    }


    public async Task<Result<CollateralResponse>> GetCollateralByIdAsync(Guid? collateralId)
    {
        return await collateralsService.GetCollateralByIdAsync(collateralId);
    }

    public async Task<IEnumerable<CollateralLineResponse>> GetCollateralLinesAsync(Guid? collateralId)
    {
        return await collateralsService.GetCollateralLinesAsync(collateralId);
    }

    public async Task<Pager<VCollateral>> GetCollateralsAysnc(int pageIndex, int pageSize, string sortColumn, bool sortByAscending, string? q = null)
    {
        return await collateralsService.GetCollateralsAysnc(pageIndex, pageSize, sortColumn, sortByAscending, q);
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
}
