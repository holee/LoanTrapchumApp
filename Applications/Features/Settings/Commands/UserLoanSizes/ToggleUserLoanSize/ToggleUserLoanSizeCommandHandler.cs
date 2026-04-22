using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.ToggleUserLoanSize;

public sealed class ToggleUserLoanSizeCommandHandler(IAppSettingRepository repository)
    : IRequestHandler<ToggleUserLoanSizeCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(ToggleUserLoanSizeCommand request, CancellationToken cancellationToken)
        => repository.ToggleUserLoanSizeActiveAsync(request.Id, request.UserRefId, cancellationToken);
}
