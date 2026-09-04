using Application.Interfaces.Identity;
using ErrorOr;

namespace Application.Features.Identity.Command.ChangePassword;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<string>>
{
    private readonly IUserService _userService;

    public ChangePasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangePassword(request.Request, cancellationToken);
        return result;
    }
}