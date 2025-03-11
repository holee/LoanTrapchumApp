using System.Security.Claims;

namespace Applications.Extensions.Identity
{
    public class RolePermissionCreateDto
    {
        public string RoleName { get; set; } = default!;
        public List<Claim>? Permissions  { get; set; }
    }
}
