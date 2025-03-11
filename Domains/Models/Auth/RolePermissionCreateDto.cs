using System.Security.Claims;

namespace Domains.Models.Auth
{
    public class RolePermissionCreateDto
    {
        public string RoleName { get; set; } = default!;
        public List<Claim>? Permissions  { get; set; }
    }
}
