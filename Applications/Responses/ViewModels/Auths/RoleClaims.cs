using Applications.Requests.Identity;

namespace Applications.Responses.ViewModels.Auths;

public class RoleClaims(long roleId, string roleName, List<RoleClaimDto> permissions)
{

    public RoleClaims() : this(0, string.Empty, [])
    {
            
    }
    public long RoleId { get; set; } = roleId;
    public string RoleName { get; set; }=roleName;
    public List<RoleClaimDto> Permissions { get; set; } = permissions;
}
