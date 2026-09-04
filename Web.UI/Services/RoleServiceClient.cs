using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Roles.Responses;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using ErrorOr;

namespace Web.UI.Services;

public sealed class RoleServiceClient(HttpClient httpClient) : IRoleService
{
    public Task<ErrorOr<Success>> AddRolePermissions(RolePermissionRequest permission) => Task.FromResult<ErrorOr<Success>>(Result.Success);
    public Task<ErrorOr<Success>> CreateAsync(RoleCreateDto role) => Task.FromResult<ErrorOr<Success>>(Result.Success);
    public Task<ErrorOr<RoleClaims>> CreateAsync(RoleCreateDto role, CancellationToken cancellationToken = default) => Task.FromResult<ErrorOr<RoleClaims>>(new RoleClaims());
    public Task<ErrorOr<RoleDtoUpdate>> EditAsync(string? roleId) => Task.FromResult<ErrorOr<RoleDtoUpdate>>(new RoleDtoUpdate());
    public List<RolePermissionsViewModel> GetAllPermisses() => [];
    public List<PermissionsDtoCreate> GetAllPermissions() => [];
    public Task<ErrorOr<RolePermissions>> GetPermissionsByRoleAsync(string roleId) => Task.FromResult<ErrorOr<RolePermissions>>(new RolePermissions());
    public Task<ErrorOr<RolePermissions>> GetPermissionsByRoleByIdAsync(string roleId) => Task.FromResult<ErrorOr<RolePermissions>>(new RolePermissions());
    public Task<ErrorOr<RoleDto>> GetRoleByIdAsync(string id) => Task.FromResult<ErrorOr<RoleDto>>(new RoleDto());
    public Task<ErrorOr<IEnumerable<RoleDto>>> GetRolesAsync(string roleId, string? name = null) => Task.FromResult((ErrorOr<IEnumerable<RoleDto>>)new List<RoleDto>());
    public Task<ErrorOr<IEnumerable<string>>> GetRolesAsync(string? name = null) => Task.FromResult<ErrorOr<IEnumerable<string>>>(new List<string> { "Credit Officer", "Admin" });
    public Task<Pager<RoleDto>> GetRolesAsync(string? roleId, QueryParam param, CancellationToken cancellation = default) => Task.FromResult(new Pager<RoleDto>());
    public Task<Pager<RoleDto>> GetRolesAsync(QueryParam param, CancellationToken cancellation = default) => Task.FromResult(new Pager<RoleDto>());
    public Task<ErrorOr<string>> UpdateAsync(RoleDtoUpdate role) => Task.FromResult<ErrorOr<string>>(Error.Unexpected());
    public Task<ErrorOr<Deleted>> DeleteRoleAsync(string roleId) => Task.FromResult<ErrorOr<Deleted>>(Result.Deleted);
}
