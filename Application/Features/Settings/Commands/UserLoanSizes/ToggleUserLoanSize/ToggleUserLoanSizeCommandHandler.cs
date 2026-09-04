using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Commands.UserLoanSizes.ToggleUserLoanSize;

public sealed class ToggleUserLoanSizeCommandHandler(IAppSettingService repository)
    : IRequestHandler<ToggleUserLoanSizeCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(ToggleUserLoanSizeCommand request, CancellationToken cancellationToken)
        => repository.ToggleUserLoanSizeActiveAsync(request.Id, request.UserRefId, cancellationToken);
}
