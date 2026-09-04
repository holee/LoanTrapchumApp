using Applications.Features.Identity.Users;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Services.Identity;
using Domains.Models.Files;
using ErrorOr;

namespace Web.UI.Services;

public sealed class UserServiceClient(HttpClient httpClient) : IUserServiceImpl
{
    public Task<ErrorOr<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission) => Task.FromResult<ErrorOr<string>>(Error.Unexpected());
    public Task<ErrorOr<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default) => Task.FromResult<ErrorOr<string>>(Error.Unexpected());
    public Task<ErrorOr<FileModel>> ChangeProfile(string? userId, ProfileDtoCreate req) => Task.FromResult<ErrorOr<FileModel>>(Error.Unexpected());
    public Task<ErrorOr<string>> CreateUserAsync(RegisterRequest model) => Task.FromResult<ErrorOr<string>>(Error.Unexpected());
    public Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam req) => Task.FromResult(new Pager<PermissionsVm>());
    public Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default) => Task.FromResult<ErrorOr<VUser>>(Error.NotFound());
    public Task<ErrorOr<UserDto>> GetUserDetailsById(string? userId) => Task.FromResult<ErrorOr<UserDto>>(Error.NotFound());
    public Task<ErrorOr<UserPermissions>> GetUserPermissionsAsync(string? userId) => Task.FromResult<ErrorOr<UserPermissions>>(new UserPermissions());
    public Task<ErrorOr<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default) => Task.FromResult<ErrorOr<UserProfileResponse>>(new UserProfileResponse());
    public Task<ErrorOr<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param) => Task.FromResult<ErrorOr<Pager<VUser>>>(new Pager<VUser>());
    public Task<ErrorOr<Updated>> UpdateUserRoleAsync(UserRoleDtoUpdate req) => Task.FromResult<ErrorOr<Updated>>(Result.Updated);
}
