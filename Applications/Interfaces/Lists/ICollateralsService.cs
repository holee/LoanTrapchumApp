using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Interfaces.Lists;

public interface ICollateralsService
{
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId, string? q = null);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id, string? searchText = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Result<CollateralResponse>> GetCollateralByIdAsync(Guid? collateralId);
    Task<IEnumerable<CollateralLineResponse>> GetCollateralLinesAsync(Guid? collateralId);
    Task<Result<CollateralResponse>> GetCollateralByCustomerAsync(Guid? customerId);
    Task<Pager<VCollateral>> GetCollateralsAysnc(int pageIndex, int pageSize, string sortColumn, bool sortByAscending, string? q = null);
    Task<Result<string>> InsertOrUpdate(Collateral entity);
}
