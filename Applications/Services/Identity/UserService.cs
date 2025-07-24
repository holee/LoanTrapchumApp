using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Files;
using FluentValidation;

namespace Applications.Services.Identity;


public sealed class UserService(IUserService userService, IValidator<ChangePasswordDto> validator) : IUserServiceImpl
{

    public async Task<Result<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission)
    {
        return await userService.AddOrRemovedUserPermissionsAsync(userId, permission);
    }

    public async Task<Result<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId)
    {
        return await userService.AddOrRemovedUserRolesAsync(useroles, userId);
    }

    public async Task<Result<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default)
    {

        var valid = validator.Validate(req);

        if (!valid.IsValid)
        {

            var errors = valid.ToDictionary();

            return Result.Failure<string>(ErrorResponse.Validation(errors));
        }

        return await userService.ChangePassword(req, cancellation);
    }


    public async Task<Result<FileModel>> ChangeProfile(string? userId, ProfileDtoCreate req)
    {

        var model = new FileModel(req.FileName, req.FileName, req.FileSize, req.FileContent);

        return await userService.ChangeProfile(userId, model);

    }

    public async Task<Result<string>> CreateUserAsync(RegisterRequest model)
    {
        return await userService.CreateUserAsync(model);
    }

    public async Task<Result> EditUserAsync(string userId, EditUserDto model)
    {
        return await userService.EditUserAsync(userId, model);
    }

    public async Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId)
    {
        return await userService.GetUserPermissionsAsync(userId);
    }

    public async Task<Result<UserViewModel>> GetUserRolesAsync(string? userId)
    {
        return await userService.GetUserRolesAsync(userId);
    }

    public async Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null)
    {
        return await GetUsersAsync(pageIndex, pageSize, sortColumn, includeInactive, filter);
    }

    public async Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null)
    {
        return await userService.GetUsersAsync(param, userId);
    }

    public async Task<Result<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param)
    {
        return await userService.GetUsersAsync(branchId, userId, param);
    }

    public async Task<Result<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null)
    {
        return await userService.GetUsersAsync(userId, filter);
    }

    public async Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default)
    {
        return await userService.GetUserByIdAsync(userId, cancellation);
    }
    public async Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default, params IEnumerable<string> columns)
    {
        return await userService.GetUserByIdAsync(userId, cancellation, columns);
    }

    public async Task<Result<UserDto>> GetUserDetailsById(string? userId) => await userService.GetUserDetailsById(userId);

    public async Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req) => await userService.UpdateUserRoleAsync(req);

    public async Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam req) => await userService.GetPermissionsAsync(req);


    public async Task<Result<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<UserProfileResponse>(ErrorResponse.Validation("No UserId"));
        }

        return await userService.GetUserProfile(userId);

    }
}
