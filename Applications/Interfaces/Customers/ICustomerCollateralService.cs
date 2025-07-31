using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Interfaces.Customers
{
    public interface ICustomerCollateralService
    {
        Task<Result<string>> DeleteCollateralLineAsync(int? id, string? collateralId, CancellationToken cancellation = default);
        Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId);
        Task<IEnumerable<VCollateralLines>> GetCollateralLinesAsync(string? collateralId);
        Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? customerId);
        Task<Result<VCollateralLines>> GetCustomerCollateralLineAsync(long? id, string? collateralId);
        Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLinesAsync(string? collateralId);
        Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId);
        Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed);
        Task<Result<string>> InsertOrUpdate(Collateral req, bool detarching, CancellationToken cancellation = default);
        Task<Result<string>> UpsertCollateralLineAsync(CollateralLine model, CancellationToken cancellation = default);
    }
}