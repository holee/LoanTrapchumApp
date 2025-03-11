namespace Applications.Requests.Identity;

public class RoleClaimDtoAssign 
{
    public long RoleId { get; set; }
    public List<RoleClaimDto> Permissions { get; set; } = [];
}