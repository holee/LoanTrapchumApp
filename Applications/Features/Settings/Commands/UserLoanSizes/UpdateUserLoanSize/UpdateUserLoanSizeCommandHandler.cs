using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.UpdateUserLoanSize;

public sealed class UpdateUserLoanSizeCommandHandler(IAppSettingRepository repository)
    : IRequestHandler<UpdateUserLoanSizeCommand, ErrorOr<UserLoanSizeDto>>
{
    public Task<ErrorOr<UserLoanSizeDto>> Handle(UpdateUserLoanSizeCommand request, CancellationToken cancellationToken)
        => repository.UpdateUserLoanSizeAsync(request.Request, cancellationToken);
}
