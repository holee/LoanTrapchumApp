using Applications.Features.Collaterals.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Queries.GetCollateralDetails;

public record GetCollateralDetailQuery(string CustomerId, bool Include = true) : IRequest<ErrorOr<VCustomerCollaterals>>;
