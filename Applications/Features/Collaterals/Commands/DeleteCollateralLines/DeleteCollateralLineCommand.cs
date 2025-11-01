using Applications.Bases;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Collaterals.Commands.DeleteCollateralLines;

public record DeleteCollateralLineCommand(long Id, string CollateralId) : IRequest<ErrorOr<DeleteResult>>;

