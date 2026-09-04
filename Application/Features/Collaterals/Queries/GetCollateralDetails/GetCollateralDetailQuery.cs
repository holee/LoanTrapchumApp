using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralDetails;

public record GetCollateralDetailQuery(string CustomerId, bool Include = true) : IRequest<ErrorOr<VCustomerCollaterals>>;
