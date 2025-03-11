namespace Applications.Responses.ViewModels.Auths;

public sealed class RolePermissionsViewModel
{
    public string RoleId { get; set; }=string.Empty;
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public bool IsAssignedToRole { get; set; } = false;
}
