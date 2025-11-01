using Applications.Utils;
using Domains.Models.Collaterals;
using ErrorOr;
namespace Applications.Features.Collaterals.Queries.GetCollateralsById;

public record GetCollateralByIdQuery(string CollateralId) : IRequest<ErrorOr<Collateral>>;
