using Applications.Features.Collaterals.Dtos;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;

namespace Applications.Interfaces.Lists;

public interface ICollateralsService
{
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? id, string? q = null);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id, string? searchText = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param,CancellationToken cancellation=default);
    Task<Result<string>> InsertOrUpdateAsync(CollateralSubType req, CancellationToken cancellation = default);
    Task<Result<string>> CreateAsync(CollateralLine req, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(CollateralLine req, CancellationToken cancellation = default);
    Task<Result<string>> UpSertAsync(CollateralLine req, CancellationToken cancellation = default);
    Task<Result<string>> Upsert(CollateralSubType req,CancellationToken cancellation=default);
}
