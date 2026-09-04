using Application.Features.Identity.Users;
using Application.Responses.Auths;
using Domains.Models.Files;
using ErrorOr;

namespace Application.Services.Identity;

public interface IUserServiceImpl
{
    Task<ErrorOr<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission);
    //Task<ErrorOr<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId);
    Task<ErrorOr<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default);
    Task<ErrorOr<FileModel>> ChangeProfile(string? userId, ProfileDtoCreate req);
    Task<ErrorOr<string>> CreateUserAsync(RegisterRequest model);
    //Task<ErrorOr<Updated>> EditUserAsync(string userId, EditUserDto model);
    Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam req);
    Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default);
    //Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default, params IEnumerable<string> columns);
    Task<ErrorOr<UserDto>> GetUserDetailsById(string? userId);
    Task<ErrorOr<UserPermissions>> GetUserPermissionsAsync(string? userId);
    Task<ErrorOr<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default);
    //Task<ErrorOr<UserViewModel>> GetUserRolesAsync(string? userId);
    //Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null);
    //Task<ErrorOr<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null);
    //Task<ErrorOr<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null);
    Task<ErrorOr<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param);
    Task<ErrorOr<Updated>> UpdateUserRoleAsync(UserRoleDtoUpdate req);
}
