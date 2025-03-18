
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;

namespace Applications.Services.Identity;


public sealed class UserService : IUserService
{
    private readonly IUserService _userService;

    public UserService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission)
    {
        return await _userService.AddOrRemovedUserPermissionsAsync(userId, permission);
    }

    public async Task<Result<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId)
    {
        return await _userService.AddOrRemovedUserRolesAsync(useroles, userId);
    }

    public async Task<Result> ChangePassword(ChangePasswordDto req)
    {
        return await _userService.ChangePassword(req);
    }

    public async Task<Result<string>> CreateUserAsync(RegisterRequest model)
    {
        return await _userService.CreateUserAsync(model);
    }

    public async Task<Result> EditUserAsync(string userId, EditUserDto model)
    {
        return await _userService.EditUserAsync(userId, model);
    }

    public async Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId)
    {
        return await _userService.GetUserPermissionsAsync(userId);
    }

    public async Task<Result<UserViewModel>> GetUserRolesAsync(string? userId)
    {
        return await _userService.GetUserRolesAsync(userId);
    }

    public async Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null)
    {
        return await GetUsersAsync(pageIndex, pageSize, sortColumn, includeInactive, filter);
    }

    public async Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null)
    {
        return await _userService.GetUsersAsync(param, userId);
    }

    public async Task<Result<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null)
    {
        return await _userService.GetUsersAsync(userId, filter);
    }

    public async Task<Result<UserDto>> GetUserDetailsById(string? userId)
    {
        return await _userService.GetUserDetailsById(userId);
    }

    public async Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req)
    {
        return await _userService.UpdateUserRoleAsync(req);
    }
}
