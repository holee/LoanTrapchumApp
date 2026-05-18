using ErrorOr;

namespace Applications.Features.Loans.Command.WidrawCollateral;

public record WidrawalCollateralCommand(IEnumerable<WidthrawCollateralDtoCreate> Request) : IRequest<ErrorOr<bool>>;


