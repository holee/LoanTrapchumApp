using Application.Features.Collaterals.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralLineDetails;

public record GetCollateralLineDetailQuery(long Id,string CollateralId) : IRequest<ErrorOr<VCollateralLines>>;
