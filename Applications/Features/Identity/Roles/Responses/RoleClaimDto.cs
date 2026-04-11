namespace Applications.Features.Identity.Roles.Responses;

public class RoleClaimDto
{
    public RoleClaimDto()
    {
        ClaimType = string.Empty;
        IsSelected = false;
    }
    public RoleClaimDto(string claimType, bool isSelected = false)
    {
        ClaimType= claimType;
        IsSelected= isSelected;    
    }

    public string? ClaimType { get; set; } = string.Empty;
    public bool IsSelected { get; set; } = false;
}
