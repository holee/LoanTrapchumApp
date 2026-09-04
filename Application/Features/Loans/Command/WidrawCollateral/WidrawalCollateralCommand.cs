using ErrorOr;

namespace Application.Features.Loans.Command.WidrawCollateral;

public record WidrawalCollateralCommand(IEnumerable<WidthrawCollateralDtoCreate> Request) : IRequest<ErrorOr<bool>>;


