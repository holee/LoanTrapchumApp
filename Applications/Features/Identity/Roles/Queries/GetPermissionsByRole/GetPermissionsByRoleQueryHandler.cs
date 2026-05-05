using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using ErrorOr;

namespace Applications.Features.Identity.Roles.Queries.GetPermissionsByRole;

public sealed class GetPermissionsByRoleQueryHandler : IRequestHandler<GetPermissionsByRoleQuery, ErrorOr<RolePermissions>>
{
    private readonly IRoleService _roleService;

    public GetPermissionsByRoleQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ErrorOr<RolePermissions>> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
        => await _roleService.GetPermissionsByRoleByIdAsync(request.RoleId);
}
