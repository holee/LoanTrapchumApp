using Application.Responses.ViewModels.Auths;

namespace Application.Responses.Auths
{
    public sealed class RolePermissions
    {
        public RoleResponse? Role { get; set; }
        public List<RolePermissionsViewModel>? Permissions { get; set; }

        public RolePermissions(RoleResponse? role, List<RolePermissionsViewModel>? permissions)
        {
            this.Role = role;
            this.Permissions = permissions;
        }
        public RolePermissions()
        {
            Role = new();
            Permissions = [];
        }
    }


}

