using Microsoft.AspNetCore.Identity;

namespace Applications.Extensions.Identity;
public class ApplicationRole:IdentityRole
{
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}
