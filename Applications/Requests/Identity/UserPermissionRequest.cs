using Applications.Responses.ViewModels.Auths;

namespace Applications.Requests.Identity;

public class UserPermissionRequest(long userId, List<UserPermissionViewModel> permissions)
{
    public long UserId { get; } = userId;
    public List<UserPermissionViewModel> Permissions { get; } = permissions;  
}