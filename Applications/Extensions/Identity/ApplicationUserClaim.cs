using Microsoft.AspNetCore.Identity;

namespace Applications.Extensions.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public string? Group { get; set; } = string.Empty;
    }
}
