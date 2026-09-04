using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Users
{
    public class EditUserDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
