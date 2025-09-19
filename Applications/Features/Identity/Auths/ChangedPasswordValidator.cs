using Applications.Features.Identity.Users;
using FluentValidation;

namespace Applications.Requests.Validations.Auths
{
    public class ChangedPasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangedPasswordValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required");

            RuleFor(x => x.CurrentPassword).Cascade(CascadeMode.Stop)
                                           .NotEmpty().WithMessage("Please enter current password")
                                           .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");

            RuleFor(x => x.NewPassword)
                                    .Cascade(CascadeMode.Stop)
                                    .NotEmpty().WithMessage("Please enter new password")
                                       .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");

            RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Please confirm new password")
                                           .MinimumLength(4).WithMessage("Password must be at least 4 characters long")
                                           .Equal(x => x.NewPassword).WithMessage("New Password and Confirm Password must match.");
        }
    }
}
