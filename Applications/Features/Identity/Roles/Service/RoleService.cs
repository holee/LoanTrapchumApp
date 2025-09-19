using Applications.Interfaces.Identity;
using Applications.Features.Identity.Roles.Requests;

namespace Applications.Features.Identity.Roles.Service
{
    public sealed class RoleService
    {
        private readonly IRoleService _roleService; 

        public RoleService(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task UpdateRoleAsync(RoleDtoUpdate role)
        {

        }

    }
}
