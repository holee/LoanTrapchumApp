namespace Application.Features.Notifications.Queries.GetNotification;

public sealed class MarkUnreadNotificationCommandValidator : AbstractValidator<MarkUnreadNotificationCommand>
{
    public MarkUnreadNotificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Notification Id is required.");
        RuleFor(x => x.BranchId)
            .GreaterThan(0).When(x => x.BranchId.HasValue).WithMessage("Branch Id must be greater than zero if provided.");
    }
}