using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.ToggleUserLoanSize;

public record ToggleUserLoanSizeCommand(long Id, string UserRefId) : IRequest<ErrorOr<bool>>;
