namespace Applications.Requests.Identity;

public class RoleClaimModuleDto 
{
    public long RoleId { get; set; }
    public Dictionary<string, List<RoleClaimDto>> RoleClaims { get; set; } = [];
}
