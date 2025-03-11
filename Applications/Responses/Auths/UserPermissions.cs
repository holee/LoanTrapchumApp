using Applications.Responses.ViewModels.Auths;

namespace Applications.Responses.Auths
{
    public sealed class UserPermissions
    {
        public UserPermissions()
        {
            UserId = string.Empty;
            Permissions = [];
        }
        public UserPermissions(string userId, List<UserPermissionViewModel> permissions)
        {
            UserId = userId;
            Permissions = permissions;
        }
        public string UserId { get; set; } = string.Empty;
        public List<UserPermissionViewModel> Permissions { get; set; } = [];
    }


}

