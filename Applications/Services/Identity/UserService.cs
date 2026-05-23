using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Files;

namespace Applications.Services.Identity;

public sealed class UserService(IUserService userService, IValidator<ChangePasswordDto> validator) : IUserServiceImpl
{
    public async Task<Result<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission)
    {
        var result = await userService.AddOrRemovedUserPermissionsAsync(userId, permission);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> useroles, string userId)
    {
        var result = await userService.AddOrRemovedUserRolesAsync(useroles, userId);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default)
    {
        var valid = validator.Validate(req);
        if (!valid.IsValid)
            return Result.Failure<string>(ErrorResponse.Validation(valid.ToDictionary()));

        var result = await userService.ChangePassword(req, cancellation);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<FileModel>> ChangeProfile(string? userId, ProfileDtoCreate req)
    {
        var model = new FileModel(req.FileName, req.FileName, req.FileSize, req.FileContent);
        var result = await userService.ChangeProfile(userId, model);
        return result.IsError
            ? Result.Failure<FileModel>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<string>> CreateUserAsync(RegisterRequest model)
    {
        var result = await userService.CreateAsync(model);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result> EditUserAsync(string userId, EditUserDto model)
    {
        var result = await userService.EditUserAsync(userId, model);
        return result.IsError
            ? Result.Failure(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success();
    }

    public async Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId)
    {
        var result = await userService.GetUserPermissionsAsync(userId);
        return result.IsError
            ? Result.Failure<UserPermissions>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<UserViewModel>> GetUserRolesAsync(string? userId)
    {
        var result = await userService.GetUserRolesAsync(userId);
        return result.IsError
            ? Result.Failure<UserViewModel>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public Task<Pager<UserDto>> GetUsersAsync(int pageIndex, int pageSize, string sortColumn, bool includeInactive = false, string? filter = null)
        => Task.FromResult(new Pager<UserDto>([], 0));

    public async Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null)
    {
        var result = await userService.GetUsersAsync(param, userId);
        return result.IsError
            ? Result.Failure<Pager<VUser>>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param)
    {
        var result = await userService.GetUsersAsync(branchId, userId, param);
        return result.IsError
            ? Result.Failure<Pager<VUser>>(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default)
    {
        var result = await userService.GetUserByIdAsync(userId, cancellation);
        return result.IsError
            ? Result.Failure<VUser>(ErrorResponse.NotFound(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<UserDto>> GetUserDetailsById(string? userId)
    {
        var result = await userService.GetUserDetailsById(userId);
        return result.IsError
            ? Result.Failure<UserDto>(ErrorResponse.NotFound(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req)
    {
        var result = await userService.UpdateUserRoleAsync(req);
        return result.IsError
            ? Result.Failure(ErrorResponse.Failure(result.FirstError.Description))
            : Result.Success();
    }

    public async Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam req)
        => await userService.GetPermissionsAsync(req);

    public async Task<Result<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(userId))
            return Result.Failure<UserProfileResponse>(ErrorResponse.Validation("No UserId"));

        var result = await userService.GetUserProfile(userId, cancellation);
        return result.IsError
            ? Result.Failure<UserProfileResponse>(ErrorResponse.NotFound(result.FirstError.Description))
            : Result.Success(result.Value);
    }
}
