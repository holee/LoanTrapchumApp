using Applications.Features.Collaterals.Dtos;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;

namespace Applications.Services.Lists;

public interface ICollateralsServiceImpl
{
    Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId);
    Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param);
    Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id = 0, string? searchText = null);
    Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId = 0, string? q = null);
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<Result<string>> UpdateAsync(CollateralSubType entity);
    Task<Result<string>> UpsertAsync(CollateralSubTypeDtoCreate entity);
    Task<Result<string>> CreateAsync(CollateralDtoCreate entity, bool detarching=false);
}
