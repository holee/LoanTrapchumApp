using Applications.Features.Identity.Roles.Responses;

namespace Applications.Features.Identity.Roles.Requests;

public class RoleClaimDtoAssign 
{
    public long RoleId { get; set; }
    public List<RoleClaimDto> Permissions { get; set; } = [];
}