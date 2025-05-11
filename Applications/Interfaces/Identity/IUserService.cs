using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;

namespace Applications.Interfaces.Identity;

public interface IUserService 
{
    Task<Result<string>> CreateUserAsync(RegisterRequest model);
    Task<Result> EditUserAsync(string userId, EditUserDto model);
    Task<Result> ChangePassword(ChangePasswordDto req);
    Task<Result<UserDto>> GetUserDetailsById(string? userId);
    //Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null);
    Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId);
    Task<Result<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission);
    Task<Result<UserViewModel>> GetUserRolesAsync(string? userId);
    Task<Result<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId);
    Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null);
    Task<Result<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null);
    Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req);

}
