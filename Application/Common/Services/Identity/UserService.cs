using Application.Features.Identity.Users;
using Application.Interfaces.Identity;
using Application.Responses.Auths;
using Domains.Models.Files;
using ErrorOr;

namespace Application.Services.Identity;

public sealed class UserService(IUserService userService, IValidator<ChangePasswordDto> validator) : IUserServiceImpl
{
    public async Task<ErrorOr<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission)
    {
        var result = await userService.AddOrRemovedUserPermissionsAsync(userId, permission);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default)
    {
        var valid = validator.Validate(req);
        if (!valid.IsValid)
            return Error.Validation(description: string.Join(", ", valid.Errors.Select(e => e.ErrorMessage)));

        var result = await userService.ChangePassword(req, cancellation);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<FileModel>> ChangeProfile(string? userId, ProfileDtoCreate req)
    {
        var model = new FileModel(req.FileName, req.FileName, req.FileSize, req.FileContent);
        var result = await userService.ChangeProfile(userId, model);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<string>> CreateUserAsync(RegisterRequest model)
    {
        var result = await userService.CreateAsync(model);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<UserPermissions>> GetUserPermissionsAsync(string? userId)
    {
        var result = await userService.GetUserPermissionsAsync(userId);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param)
    {
        var result = await userService.GetUsersAsync(branchId, userId, param);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default)
    {
        var result = await userService.GetUserByIdAsync(userId, cancellation);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<UserDto>> GetUserDetailsById(string? userId)
    {
        var result = await userService.GetUserDetailsById(userId);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<ErrorOr<Updated>> UpdateUserRoleAsync(UserRoleDtoUpdate req)
    {
        var result = await userService.UpdateUserRoleAsync(req);
        return result.IsError ? result.FirstError : result.Value;
    }

    public async Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam req)
        => await userService.GetPermissionsAsync(req);

    public async Task<ErrorOr<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Validation(description: "No UserId");

        var result = await userService.GetUserProfile(userId, cancellation);
        return result.IsError ? result.FirstError : result.Value;
    }
}
