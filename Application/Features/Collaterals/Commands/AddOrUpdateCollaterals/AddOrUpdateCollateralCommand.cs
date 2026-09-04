using Application.Features.Collaterals.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Commands.AddOrUpdateCollaterals;

public record AddOrUpdateCollateralCommand(CollateralDtoCreate Request, bool Detaching = false) : IRequest<ErrorOr<string>>;
