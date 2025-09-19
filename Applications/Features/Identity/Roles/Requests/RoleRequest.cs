using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Identity.Roles.Requests
{
    public class RoleRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }=string.Empty;
        [Required]
        public string Role { get; set; }=string.Empty;
    }
}
