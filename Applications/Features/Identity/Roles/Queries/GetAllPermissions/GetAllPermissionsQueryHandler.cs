using Applications.Interfaces.Identity;
using Applications.Responses.ViewModels.Auths;

namespace Applications.Features.Identity.Roles.Queries.GetAllPermissions;

public sealed class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionsDtoCreate>>
{
    private readonly IRoleService _roleService;

    public GetAllPermissionsQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public Task<List<PermissionsDtoCreate>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        => Task.FromResult(_roleService.GetAllPermissions());
}
