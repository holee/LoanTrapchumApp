using Applications.Bases;
using Applications.Features.Collaterals.Dtos;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using ErrorOr;

namespace Applications.Interfaces.Customers;

public interface ICollateralService
{
    Task<ErrorOr<DeleteResult>> DeleteCollateralLineAsync(long? id, string? collateralId,
        CancellationToken cancellation = default);

    Task<ErrorOr<Collateral>> GetCollateralByIdAsync(string? customerId, CancellationToken cancellation = default);

    Task<ErrorOr<VCustomerCollaterals>> GetCollateralDetailAsync(string? customerId, bool includeInUsed = true,
        CancellationToken cancellation = default);

    Task<ErrorOr<IEnumerable<VCollateralLines>>> GetCollateralLinesAsync(string? collateralId, CancellationToken cancellation = default);
    Task<ErrorOr<VCollateralLines>> GetCollateralLinesAsync(long? id, string? collateralId);

    Task<ErrorOr<VCollateralLines>> GetCollateralLineDetailAsync(long? id, string? collateralId,
        CancellationToken cancellation = default);

    Task<ErrorOr<string>> AddOrUpdateAsync(Collateral req, bool detarching, CancellationToken cancellation = default);

    Task<ErrorOr<string>> InsertOrUpdateCollateralLineAsync(CollateralLine model,
        CancellationToken cancellation = default);

    Task<ErrorOr<CollateralLine>> GetCollateralLineByIdAsync(long? id, string? collateralId);
    Task<ErrorOr<Collateral>> GetEditCollateralByIdAsync(long lineId, string? customerId, CancellationToken cancellation = default);
    Task<Pager<VCollateralLines>> GetCollateralLinesAsPaging(string? customerId, QueryParam query, CancellationToken cancellationToken);
}


//Task<ErrorOr<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId);