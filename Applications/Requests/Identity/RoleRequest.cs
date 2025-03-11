using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Identity
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
