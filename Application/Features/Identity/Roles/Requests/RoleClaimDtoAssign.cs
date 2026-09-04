using Application.Features.Identity.Roles.Responses;

namespace Application.Features.Identity.Roles.Requests;

public class RoleClaimDtoAssign 
{
    public long RoleId { get; set; }
    public List<RoleClaimDto> Permissions { get; set; } = [];
}