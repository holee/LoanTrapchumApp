namespace Applications.Features.Identity.Command.CreateNewUser;

public sealed class CreateNewUserCommandValidator : AbstractValidator<CreateNewUserCommand>
{
    public CreateNewUserCommandValidator()
    {
        RuleFor(x => x.Request.Email).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Email is required.")
                .NotEmpty()
                    .WithMessage("Email cannot be empty.")
                .EmailAddress()
                    .WithMessage("Email is not valid.");
        RuleFor(x => x.Request.Password).Cascade(CascadeMode.Stop)
                    .NotEmpty()
                        .WithMessage("Password is required.")
                    .MinimumLength(4)
                        .WithMessage("Password must be at least 4 characters long.");
        RuleFor(x => x.Request.ConfirmPassword).Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Confirm password is required.")
            .Equal(x => x.Request.Password)
                .WithMessage("Confirm password must match the password.");
    }
}