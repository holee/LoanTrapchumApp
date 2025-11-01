using Applications.Features.Collaterals.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Queries.GetCollateralLineDetails;

public record GetCollateralLineDetailQuery(long Id,string CollateralId) : IRequest<ErrorOr<VCollateralLines>>;
