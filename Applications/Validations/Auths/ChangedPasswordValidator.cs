using Applications.Requests.Identity;
using FluentValidation;
namespace Applications.Validations.Auths
{
    public class ChangedPasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangedPasswordValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required");
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Please enter new password")
                                           .MinimumLength(4).WithMessage("Password length is not meet requirement.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Please enter new password")
                                           .MinimumLength(4).WithMessage("Password length is not meet requirement.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please enter new password")
                                           .MinimumLength(4).WithMessage("Password length is not meet requirement.")
                                           .Equal("NewPassword").WithMessage("New Password and Confirm Password not same.");
        }
    }
}
