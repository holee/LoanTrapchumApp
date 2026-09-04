using Application.Responses.ViewModels.Auths;

namespace Application.Features.Identity.Users;

public class RolePermissionRequest(string RoleId, List<RolePermissionsViewModel>? RoleClaims)
{
    public string RoleId { get; } = RoleId;
    public List<RolePermissionsViewModel>? RoleClaims { get; } = RoleClaims;
}
