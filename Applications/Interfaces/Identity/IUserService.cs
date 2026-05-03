using Applications.Features.Identity.Users;
using Applications.Responses.Auths;
using Domains.Models.Files;
using ErrorOr;

namespace Applications.Interfaces.Identity;

public interface IUserService
{
    Task<ErrorOr<string>> CreateUserAsync(RegisterRequest model, CancellationToken cancellation = default);
    Task<ErrorOr<Updated>> EditUserAsync(string userId, EditUserDto model, CancellationToken cancellation = default);
    Task<ErrorOr<UserDto>> GetUserDetailsById(string? userId, CancellationToken cancellation = default);
    Task<ErrorOr<UserPermissions>> GetUserPermissionsAsync(string? userId, CancellationToken cancellation = default);
    Task<ErrorOr<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission, CancellationToken cancellation = default);
    Task<ErrorOr<UserViewModel>> GetUserRolesAsync(string? userId, CancellationToken cancellation = default);
    Task<ErrorOr<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId, CancellationToken cancellation = default);
    Task<ErrorOr<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null, CancellationToken cancellation = default);
    //Task<ErrorOr<List<VUser>>> GetUsersAsync(string? userId, string? filter = null, CancellationToken cancellation = default);
    Task<ErrorOr<Updated>> UpdateUserRoleAsync(UserRoleDtoUpdate req, CancellationToken cancellation = default);
    Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam q, CancellationToken cancellation = default);
    Task<ErrorOr<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default);
    //Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default, params IEnumerable<string> columns);
    Task<ErrorOr<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default);
    Task<ErrorOr<FileModel>> ChangeProfile(string? userId, FileModel model, CancellationToken cancellation = default);
    Task<ErrorOr<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default);
    Task<ErrorOr<VUser>> GetUserDetailById(string? userId, CancellationToken cancellation = default);
}
