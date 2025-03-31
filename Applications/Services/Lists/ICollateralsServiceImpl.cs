using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;

namespace Applications.Services.LIsts;

public interface ICollateralsServiceImpl
{
    Task<Result<CollateralResponse>> GetCollateralByCustomerAsync(Guid? customerId);
    Task<Result<CollateralResponse>> GetCollateralByIdAsync(Guid? collateralId);
    Task<IEnumerable<CollateralLineResponse>> GetCollateralLinesAsync(Guid? collateralId);
    Task<Pager<VCollateral>> GetCollateralsAysnc(int pageIndex, int pageSize, string sortColumn, bool sortByAscending, string? q = null);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id = 0, string? searchText = null);
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId = 0, string? q = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Result<string>> CreateAsync(CollateralDtoCreate entity);
    Task<Result<string>> UpdateAsync(CollateralDtoUpdate entity); 
}
