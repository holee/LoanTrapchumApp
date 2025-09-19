using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Identity.Users
{
    public class ChangeEmailDto 
    {

        [Required]
        public long UserId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter current password.")]
        public string CurrentPassword { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter new password.")]
        public string NewPassword { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter confirm password.")]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
