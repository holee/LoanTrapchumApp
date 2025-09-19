using Applications.Responses.ViewModels.Auths;

namespace Applications.Features.Identity.Users;

public class RolePermissionRequest(string RoleId, List<RolePermissionsViewModel>? RoleClaims)
{
    public string RoleId { get; } = RoleId;
    public List<RolePermissionsViewModel>? RoleClaims { get; } = RoleClaims;
}
