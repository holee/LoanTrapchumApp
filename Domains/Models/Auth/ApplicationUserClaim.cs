using Microsoft.AspNetCore.Identity;

namespace Domains.Models.Auth
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public string? Group { get; set; } = string.Empty;
    }
}
