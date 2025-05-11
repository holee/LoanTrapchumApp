using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;

namespace Applications.Services.LIsts;

public interface ICollateralsServiceImpl
{
    Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId);
    Task<IEnumerable<Responses.Lists.Collaterals.VCollateralLines>> GetCollateralLinesAsync(string? collateralId);
    Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id = 0, string? searchText = null);
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId = 0, string? q = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Result<string>> CreateAsync(CollateralDtoCreate entity);
    Task<Result<string>> UpdateAsync(CollateralDtoUpdate entity);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? customerId);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed);
}
