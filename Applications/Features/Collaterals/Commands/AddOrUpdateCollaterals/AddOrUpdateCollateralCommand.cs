using Applications.Features.Collaterals.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;

public record AddOrUpdateCollateralCommand(CollateralDtoCreate Request, bool Detaching = false) : IRequest<ErrorOr<string>>;
