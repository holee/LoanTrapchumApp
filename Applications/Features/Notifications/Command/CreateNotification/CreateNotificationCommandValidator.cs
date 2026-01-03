namespace Applications.Features.Notifications.Command.CreateNotification;

public sealed class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull().WithMessage("Notification request cannot be null.");
        RuleFor(x => x.Request.BranchId)
          .GreaterThan(0).WithMessage("Notification request cannot be null.");
        RuleFor(x => x.Request.Title)
            .NotEmpty().WithMessage("Notification title is required.")
            .MaximumLength(100).WithMessage("Notification title cannot exceed 100 characters.");
        RuleFor(x => x.Request.Message)
            .NotEmpty().WithMessage("Notification message is required.")
            .MaximumLength(500).WithMessage("Notification message cannot exceed 500 characters.");
        RuleFor(x => x.Request.ReferenceId)
            .NotEmpty().WithMessage("Reference ID is required.");
    }
}