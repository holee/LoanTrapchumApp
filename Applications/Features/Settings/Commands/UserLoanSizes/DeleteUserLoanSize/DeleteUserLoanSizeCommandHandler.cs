using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.DeleteUserLoanSize;

public sealed class DeleteUserLoanSizeCommandHandler(IAppSettingService repository)
    : IRequestHandler<DeleteUserLoanSizeCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeleteUserLoanSizeCommand request, CancellationToken cancellationToken)
        => repository.DeleteUserLoanSizeAsync(request.Id, cancellationToken);
}
