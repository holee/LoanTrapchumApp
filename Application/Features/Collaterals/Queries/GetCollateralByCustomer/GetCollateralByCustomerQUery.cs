using Application.Features.Collaterals.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralByCustomer;

public record GetCollateralByCustomerQuery(string CustomerId, bool Include = true) : IRequest<ErrorOr<VCustomerCollaterals>>;