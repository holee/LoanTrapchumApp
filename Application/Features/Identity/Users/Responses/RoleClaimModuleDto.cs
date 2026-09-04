using Application.Features.Identity.Roles.Responses;

namespace Application.Features.Identity.Users;

public class RoleClaimModuleDto 
{
    public long RoleId { get; set; }
    public Dictionary<string, List<RoleClaimDto>> RoleClaims { get; set; } = [];
}
