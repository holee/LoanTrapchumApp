using Microsoft.AspNetCore.Identity;

namespace Applications.Extensions.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string? Module { get; set; } = string.Empty;
        public string? Group { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
