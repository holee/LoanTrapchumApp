using Applications.Features.Identity.Roles.Responses;

namespace Applications.Responses.ViewModels.Auths;

public class RoleClaims(string? roleId, string? roleName, List<RoleClaimDto> permissions)
{

    public RoleClaims() : this(string.Empty, string.Empty, new List<RoleClaimDto>())
    {
            
    }
    public string? RoleId { get; set; } = roleId;
    public string? RoleName { get; set; }=roleName;
    public List<RoleClaimDto> Permissions { get; set; } = permissions;
}
