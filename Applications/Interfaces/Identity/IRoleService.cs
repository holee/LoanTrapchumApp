using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;

namespace Applications.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<Result> AddRolePermissions(RolePermissionRequest permission);
        Task<Result> CreateAsync(RoleCreateDto role);
        Task<Result<RolePermissions>> GetPermissionsByRoleAsync(string roleId);
        Task<Result<RoleDto>> GetRoleByIdAsync(string id);
        Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(string roleId, string? name = null);
        Task<Result<IEnumerable<string>>> GetRolesAsync(string? name = null);
    }
}