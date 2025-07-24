namespace Applications.Responses.ViewModels.Auths;

public sealed class RolePermissionsDto  
{
    public string? Group { get; set; }=string.Empty;
    public bool GroupCheck { get; set; } = false;
    public bool Indeterminate { get; set; } = false;
    public List<RolePermissionsViewModel> Permissions { get; set; } = [];
}
