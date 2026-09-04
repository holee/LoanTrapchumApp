namespace Application.Responses.ViewModels.Auths;

public sealed class RolePermissionsDtoCreate 
{
    public string? Group { get; set; } = string.Empty;
    public bool GroupCheck { get; set; } = false;
    public bool Indeterminate { get; set; } = false;
    public List<PermissionsDtoCreate> Permissions { get; set; } = [];
}
