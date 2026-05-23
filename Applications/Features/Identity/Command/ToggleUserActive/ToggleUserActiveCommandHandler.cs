using Applications.Interfaces.Identity;
using ErrorOr;

namespace Applications.Features.Identity.Command.ToggleUserActive;

public sealed class ToggleUserActiveCommandHandler : IRequestHandler<ToggleUserActiveCommand, ErrorOr<Updated>>
{
    private readonly IUserService _userService;

    public ToggleUserActiveCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<Updated>> Handle(ToggleUserActiveCommand request, CancellationToken cancellationToken)
        => await _userService.ToggleUserActiveAsync(request.UserId, cancellationToken);
}
