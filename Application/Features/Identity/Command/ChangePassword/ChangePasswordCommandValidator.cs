namespace Application.Features.Identity.Command.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request is required.");
        RuleFor(x => x.Request.UserId).NotNull().WithMessage("User Id is required.")
            .NotEmpty().WithMessage("User Id is required.");
        RuleFor(x => x.Request.CurrentPassword).NotNull().WithMessage("Current password is required.")
            .NotEmpty().WithMessage("Current password is required.");
        RuleFor(x => x.Request.NewPassword).NotNull().WithMessage("New password is required.")
            .NotEmpty().WithMessage("New password is required.");
    }
}
