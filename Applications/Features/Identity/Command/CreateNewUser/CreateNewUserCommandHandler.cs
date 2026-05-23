using Applications.Interfaces.Identity;
using ErrorOr;

namespace Applications.Features.Identity.Command.CreateNewUser;

public sealed class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, ErrorOr<string>>
{
    private readonly IUserService _userService;
    public CreateNewUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<string>> Handle(CreateNewUserCommand command, CancellationToken cancellationToken)
    {
        return await _userService.CreateAsync(command.Request, cancellationToken);
    }
}
