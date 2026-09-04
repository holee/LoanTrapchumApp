using Application.Interfaces.Identity;
using ErrorOr;

namespace Application.Features.Identity.Roles.Commands.AddRolePermissions;

public sealed class AddRolePermissionsCommandHandler : IRequestHandler<AddRolePermissionsCommand, ErrorOr<Success>>
{
    private readonly IRoleService _roleService;

    public AddRolePermissionsCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ErrorOr<Success>> Handle(AddRolePermissionsCommand request, CancellationToken cancellationToken)
        => await _roleService.AddRolePermissions(request.Permission);
}
