using Application.Responses.ViewModels.Auths;

namespace Application.Features.Identity.Users;

public class UserPermissionRequest(string userId, List<UserPermissionViewModel> permissions)
{
    public string UserId { get; } = userId;
    public List<UserPermissionViewModel> Permissions { get; } = permissions;  
}