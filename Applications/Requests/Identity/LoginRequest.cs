using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Identity;

public sealed class LoginRequest
{
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; } = false;
}
