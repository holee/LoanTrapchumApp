namespace Application.Responses.ViewModels.Auths;

public sealed class UserPermissionViewModel
{

    public UserPermissionViewModel() { }
    public string UserId { get; set; }=string.Empty;
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public bool IsAssignedToRole { get; set; } = false;
}
