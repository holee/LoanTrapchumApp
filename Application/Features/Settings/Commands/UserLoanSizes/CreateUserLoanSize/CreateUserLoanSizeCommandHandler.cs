using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Commands.UserLoanSizes.CreateUserLoanSize;

public sealed class CreateUserLoanSizeCommandHandler(IAppSettingService repository)
    : IRequestHandler<CreateUserLoanSizeCommand, ErrorOr<UserLoanSizeDto>>
{
    public Task<ErrorOr<UserLoanSizeDto>> Handle(CreateUserLoanSizeCommand request, CancellationToken cancellationToken)
        => repository.CreateUserLoanSizeAsync(request.Request, cancellationToken);
}
