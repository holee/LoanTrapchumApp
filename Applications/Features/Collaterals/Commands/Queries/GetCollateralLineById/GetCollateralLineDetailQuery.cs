using Applications.Responses.Lists.Collaterals;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Commands.Queries.GetCollateralLineById;
public record struct GetCollateralLineDetailQuery(long Id, string? CollateralId) :IRequest<ErrorOr<VCollateralLines>>;