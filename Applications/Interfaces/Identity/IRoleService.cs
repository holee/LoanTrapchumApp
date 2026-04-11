using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Roles.Responses;
using Applications.Features.Identity.Users;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;

namespace Applications.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<Result> AddRolePermissions(RolePermissionRequest permission);
        Task<Result> CreateAsync(RoleCreateDto role);
        Task<ErrorOr.ErrorOr<RoleClaims>> CreateAsync(RoleCreateDto role, CancellationToken cancellationToken = default);
        Task<Result<RoleDtoUpdate>> EditAsync(string? roleId);
        List<RolePermissionsViewModel> GetAllPermisses();
        List<PermissionsDtoCreate> GetAllPermissions();
        Task<Result<RolePermissions>> GetPermissionsByRoleAsync(string roleId);
        Task<Result<RolePermissions>> GetPermissionsByRoleByIdAsync(string roleId);
        Task<Result<RoleDto>> GetRoleByIdAsync(string id);
        Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(string roleId, string? name = null);
        Task<Result<IEnumerable<string>>> GetRolesAsync(string? name = null);
        Task<Pager<RoleDto>> GetRolesAsync(string? roleId, QueryParam param, CancellationToken cancellation = default);
        Task<Pager<RoleDto>> GetRolesAsync(QueryParam param, CancellationToken cancellation = default);
        Task<Result<string>> UpdateAsync(RoleDtoUpdate role);
    }
}