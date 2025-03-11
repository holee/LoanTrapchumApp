using Microsoft.AspNetCore.Identity;

namespace Domains.Models.Auth;
public class ApplicationRole:IdentityRole
{
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}
