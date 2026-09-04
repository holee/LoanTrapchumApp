using Application.Interfaces.Identity;
using ErrorOr;

namespace Application.Features.Identity.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorOr<RoleClaims>>
{
    private readonly IRoleService _roleService;
    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<ErrorOr<RoleClaims>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await _roleService.CreateAsync(request.Role, cancellationToken);

    }
}