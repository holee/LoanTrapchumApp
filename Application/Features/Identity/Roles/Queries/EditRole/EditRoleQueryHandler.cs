using Application.Features.Identity.Roles.Requests;
using Application.Interfaces.Identity;
using ErrorOr;

namespace Application.Features.Identity.Roles.Queries.EditRole;

public sealed class EditRoleQueryHandler : IRequestHandler<EditRoleQuery, ErrorOr<RoleDtoUpdate>>
{
    private readonly IRoleService _roleService;

    public EditRoleQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ErrorOr<RoleDtoUpdate>> Handle(EditRoleQuery request, CancellationToken cancellationToken)
        => await _roleService.EditAsync(request.RoleId);
}
