using Applications.Responses.ViewModels.Auths;

namespace Applications.Requests.Identity;

public class RolePermissionRequest(long RoleId, List<RolePermissionsViewModel>? RoleClaims)
{
    public long RoleId { get; } = RoleId;
    public List<RolePermissionsViewModel>? RoleClaims { get; } = RoleClaims;
}
