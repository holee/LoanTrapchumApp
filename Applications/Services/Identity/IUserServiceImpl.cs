using Applications.Features.Identity.Users;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Files;

namespace Applications.Services.Identity;

public interface IUserServiceImpl
{
    Task<Result<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission);
    Task<Result<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId);
    Task<Result<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default);
    Task<Result<FileModel>> ChangeProfile(string? userId, ProfileDtoCreate req);
    Task<Result<string>> CreateUserAsync(RegisterRequest model); 
    Task<Result> EditUserAsync(string userId, EditUserDto model);
    Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam req);
    Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default);
    Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default, params IEnumerable<string> columns);
    Task<Result<UserDto>> GetUserDetailsById(string? userId);
    Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId);
    Task<Result<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default);
    Task<Result<UserViewModel>> GetUserRolesAsync(string? userId);
    Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null);
    Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null);
    Task<Result<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null);
    Task<Result<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param);
    Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req);
}
