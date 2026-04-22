using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.CreateUserLoanSize;

public sealed class CreateUserLoanSizeCommandHandler(IAppSettingRepository repository)
    : IRequestHandler<CreateUserLoanSizeCommand, ErrorOr<UserLoanSizeDto>>
{
    public Task<ErrorOr<UserLoanSizeDto>> Handle(CreateUserLoanSizeCommand request, CancellationToken cancellationToken)
        => repository.CreateUserLoanSizeAsync(request.Request, cancellationToken);
}
