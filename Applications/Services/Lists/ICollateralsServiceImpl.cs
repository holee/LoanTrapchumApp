using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Services.LIsts;

public interface ICollateralsServiceImpl
{
    Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId);
    Task<IEnumerable<Responses.Lists.Collaterals.VCollateralLines>> GetCollateralLinesAsync(string? collateralId);
    Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id = 0, string? searchText = null);
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId = 0, string? q = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? customerId);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed);
    Task<Result<string>> UpdateAsync(CollateralSubType entity);
    Task<Result<string>> UpdateAsync(CollateralDtoUpdate entity);
    Task<Result<string>> CreateAsync(CollateralLineDtoCreate entity, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(CollateralLineDtoUpdate entity, CancellationToken cancellation = default);
    Task<Result<string>> UpsertAsync(CollateralSubTypeDtoCreate entity);
    Task<Result<string>> CreateAsync(CollateralDtoCreate entity, bool detarching=false);
}
