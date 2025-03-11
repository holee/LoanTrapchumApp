using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Interfaces.Lists;

public interface ICollateralsServiceImpl
{
    Task<IEnumerable<CollateralType?>> GetCollateralTypes(int? collateralId = 0, string? q = null);
    Task<IEnumerable<CollateralSubType?>> GetCollateralSubTypes(int? id = 0, string? searchText = null);
    Task<Pager<VCollateral>> GetCollateralsAysnc(int pageIndex, int pageSize,string sortColumn, bool sortByAscending, string? q = null);
    Task<Result<Collateral>> GetCollateralByIdAsync(long collateralId); 
    Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null);
    Task<IEnumerable<CollateralLine>> GetCollateralLinesAsync(long collateralId); 
    Task<Result<Collateral>> GetCollateralByCustomerAsync(long customerId);
    Task<Result> InsertOrUpdate(Collateral entity);
}
