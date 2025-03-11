using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;

namespace Applications.Interfaces.Identity
{
    public interface IRoleServiceImpl
    {
        Task<Result> AddRolePermissions(RolePermissionRequest permission);
        Task<Result> CreateAsync(RoleCreateDto role);
        Task<Result<RolePermissions>> GetPermissionsByRoleAsync(string roleId);
        Task<Result<RoleDto>> GetRoleByIdAsync(string id);
        Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(string id, string? name = null);
    }
}
