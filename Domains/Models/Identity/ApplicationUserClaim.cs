using Microsoft.AspNetCore.Identity;

namespace Domains.Models.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public string? Group { get; set; } = string.Empty;
    }
}
