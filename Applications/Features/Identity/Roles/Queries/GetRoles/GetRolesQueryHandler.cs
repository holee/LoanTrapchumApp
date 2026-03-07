using Applications.Interfaces.Identity;

namespace Applications.Features.Identity.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<string>>
{
    private readonly IRoleService _roleService;
    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<IEnumerable<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRolesAsync(new QueryParam(0, 0, string.Empty, request.Filter), cancellationToken);

        var roles = result.Items.Select(r => r.RoleName).OfType<string>().ToList();

        return roles;
    }
}