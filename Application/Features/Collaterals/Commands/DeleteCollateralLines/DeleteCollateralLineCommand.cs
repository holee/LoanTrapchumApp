using Application.Bases;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Collaterals.Commands.DeleteCollateralLines;

public record DeleteCollateralLineCommand(long Id, string CollateralId) : IRequest<ErrorOr<DeleteResult>>;

