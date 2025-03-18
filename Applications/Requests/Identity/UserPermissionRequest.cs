using Applications.Responses.ViewModels.Auths;

namespace Applications.Requests.Identity;

public class UserPermissionRequest(string userId, List<UserPermissionViewModel> permissions)
{
    public string UserId { get; } = userId;
    public List<UserPermissionViewModel> Permissions { get; } = permissions;  
}