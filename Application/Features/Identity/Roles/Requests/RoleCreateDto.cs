using Application.Responses.ViewModels.Auths;

namespace Application.Features.Identity.Roles.Requests;

public sealed class RoleCreateDto
{
    public string? RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<RolePermissionsViewModel> Permissions { get; set; } = []; 
}

public sealed class RoleDtoUpdate 
{
    public string RoleId { get; set; }=string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}