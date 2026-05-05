using Applications.Features.Identity.Roles.Responses;
using Applications.Interfaces.Identity;

namespace Applications.Features.Identity.Roles.Queries.GetRolesAsPaging;

public sealed class GetRolesAsPagingQueryHandler : IRequestHandler<GetRolesAsPagingQuery, Pager<RoleDto>>
{
    private readonly IRoleService _roleService;

    public GetRolesAsPagingQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Pager<RoleDto>> Handle(GetRolesAsPagingQuery request, CancellationToken cancellationToken)
        => await _roleService.GetRolesAsync(request.Param, cancellationToken);
}
