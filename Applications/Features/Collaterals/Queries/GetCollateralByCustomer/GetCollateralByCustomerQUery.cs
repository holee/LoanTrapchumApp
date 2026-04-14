using Applications.Features.Collaterals.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Queries.GetCollateralByCustomer;

public record GetCollateralByCustomerQuery(string CustomerId, bool Include = true) : IRequest<ErrorOr<VCustomerCollaterals>>;