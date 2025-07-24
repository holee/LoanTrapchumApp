namespace Applications.Responses.ViewModels.Auths;

public sealed class PermissionsDtoCreate  
{
    public string Group { get; set; } = string.Empty;
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSelected { get; set; } = false;
}
