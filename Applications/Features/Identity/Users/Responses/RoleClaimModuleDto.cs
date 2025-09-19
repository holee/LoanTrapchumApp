using Applications.Features.Identity.Roles.Responses;

namespace Applications.Features.Identity.Users;

public class RoleClaimModuleDto 
{
    public long RoleId { get; set; }
    public Dictionary<string, List<RoleClaimDto>> RoleClaims { get; set; } = [];
}
