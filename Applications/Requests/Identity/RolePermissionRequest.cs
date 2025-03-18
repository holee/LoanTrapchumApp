using Applications.Responses.ViewModels.Auths;

namespace Applications.Requests.Identity;

public class RolePermissionRequest(string RoleId, List<RolePermissionsViewModel>? RoleClaims)
{
    public string RoleId { get; } = RoleId;
    public List<RolePermissionsViewModel>? RoleClaims { get; } = RoleClaims;
}
