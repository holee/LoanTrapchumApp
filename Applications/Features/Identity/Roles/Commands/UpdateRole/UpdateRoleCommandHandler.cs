using Applications.Interfaces.Identity;
using ErrorOr;

namespace Applications.Features.Identity.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ErrorOr<string>>
{
    private readonly IRoleService _roleService;

    public UpdateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ErrorOr<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        => await _roleService.UpdateAsync(request.Role);
}
