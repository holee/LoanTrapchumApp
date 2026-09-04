using Application.Features.Identity.Roles.Requests;
using Application.Features.Identity.Roles.Responses;
using Application.Features.Identity.Users;
using Application.Responses.Auths;
using ErrorOr;

namespace Application.Interfaces.Identity;

public interface IRoleService
{
    Task<ErrorOr<Success>> AddRolePermissions(RolePermissionRequest permission);
    Task<ErrorOr<Success>> CreateAsync(RoleCreateDto role);
    Task<ErrorOr<RoleClaims>> CreateAsync(RoleCreateDto role, CancellationToken cancellationToken = default);
    Task<ErrorOr<RoleDtoUpdate>> EditAsync(string? roleId);
    List<RolePermissionsViewModel> GetAllPermisses();
    List<PermissionsDtoCreate> GetAllPermissions();
    Task<ErrorOr<RolePermissions>> GetPermissionsByRoleAsync(string roleId);
    Task<ErrorOr<RolePermissions>> GetPermissionsByRoleByIdAsync(string roleId);
    Task<ErrorOr<RoleDto>> GetRoleByIdAsync(string id);
    Task<ErrorOr<IEnumerable<RoleDto>>> GetRolesAsync(string roleId, string? name = null);
    Task<ErrorOr<IEnumerable<string>>> GetRolesAsync(string? name = null);
    Task<Pager<RoleDto>> GetRolesAsync(string? roleId, QueryParam param, CancellationToken cancellation = default);
    Task<Pager<RoleDto>> GetRolesAsync(QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<string>> UpdateAsync(RoleDtoUpdate role);
    Task<ErrorOr<Deleted>> DeleteRoleAsync(string roleId);
}