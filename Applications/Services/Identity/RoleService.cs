using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;

namespace Applications.Services.Identity
{
    public sealed class RoleService(IRoleService roleService) : IRoleService
    {
        public async Task<Result> AddRolePermissions(RolePermissionRequest permission)
        {
            return await roleService.AddRolePermissions(permission);
        }

        public async Task<Result> CreateAsync(RoleCreateDto role)
        {
            return await roleService.CreateAsync(role);
        }

        public async Task<Result<RolePermissions>> GetPermissionsByRoleAsync(string roleId)
        {
            return await roleService.GetPermissionsByRoleAsync(roleId);
        }

        public async Task<Result<RoleDto>> GetRoleByIdAsync(string id)
        {
            return await roleService.GetRoleByIdAsync(id);
        }

        public async Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(string? roleId = null, string? roleName = null)
        {
            return await roleService.GetRolesAsync(roleId, roleName);
        }
    }
}
