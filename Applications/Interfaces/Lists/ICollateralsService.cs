using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Interfaces.Lists;

public interface ICollateralsService
{
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? id, string? q = null);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id, string? searchText = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId);
    Task<IEnumerable<Responses.Lists.Collaterals.VCollateralLines>> GetCollateralLinesAsync(string? collateralId);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? customerId);
    Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId);
    Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed);
    Task<Result<string>> InsertOrUpdateAsync(CollateralSubType req, CancellationToken cancellation = default);
    Task<Result<string>> CreateAsync(CollateralLine req, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(CollateralLine req, CancellationToken cancellation = default);
    Task<Result<string>> UpSertAsync(CollateralLine req, CancellationToken cancellation = default);
    Task<Result<string>> Upsert(CollateralSubType req,CancellationToken cancellation=default);
    Task<Result<string>> InsertOrUpdate(Collateral req, bool detarching, CancellationToken cancellation = default);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLinesAsync(string? collateralId);
    Task<Result<VCollateralLines>> GetCustomerCollateralLineAsync(long? id, string? collateralId);
}
