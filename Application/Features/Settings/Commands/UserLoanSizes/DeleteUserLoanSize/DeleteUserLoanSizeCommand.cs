using ErrorOr;

namespace Application.Features.Settings.Commands.UserLoanSizes.DeleteUserLoanSize;

public record DeleteUserLoanSizeCommand(long Id) : IRequest<ErrorOr<bool>>;
