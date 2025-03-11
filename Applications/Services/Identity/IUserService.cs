using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;

namespace Applications.Services.Identity;

public interface IUserServiceImpl 
{
    Task<Result> CreateUserAsync(RegisterRequest model);
    Task<Result> EditUserAsync(string userId, EditUserDto model);
    Task<Result> ChangePassword(ChangePasswordDto req);
    Task<Result> LockUserAccount(string email);
    Task<Result<UserDto>> GetUsertById(string? userId);
    Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null);
    Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId);
    Task<Result> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission);
    Task<Result<UserViewModel>> GetUserRolesAsync(string? userId);
    Task<Result> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId);
    Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null);
    Task<Result<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null);
    Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req);
}
