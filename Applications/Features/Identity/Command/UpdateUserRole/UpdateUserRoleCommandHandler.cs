using Applications.Interfaces.Identity;
using ErrorOr;

namespace Applications.Features.Identity.Command.UpdateUserRole;

public sealed class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, ErrorOr<Updated>>
{
    private readonly IUserService _userService;
    public UpdateUserRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<Updated>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUserRoleAsync(request.Request, cancellationToken);
        return result;
    }
}