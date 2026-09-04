using Application.Utils;
using Domains.Models.Collaterals;
using ErrorOr;
namespace Application.Features.Collaterals.Queries.GetCollateralsById;

public record GetCollateralByIdQuery(string CollateralId) : IRequest<ErrorOr<Collateral>>;
