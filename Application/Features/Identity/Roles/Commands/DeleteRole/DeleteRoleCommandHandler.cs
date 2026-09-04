using Application.Interfaces.Identity;
using ErrorOr;

namespace Application.Features.Identity.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ErrorOr<Deleted>>
{
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        => await _roleService.DeleteRoleAsync(request.RoleId);
}
