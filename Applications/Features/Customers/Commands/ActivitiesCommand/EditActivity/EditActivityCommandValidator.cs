using FluentValidation;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class EditActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public EditActivityCommandValidator()
        {
            RuleFor(command => command.ActivityDate).NotNull()
                                .WithMessage("Activity Date is required.");
            RuleFor(x=>x.CustomerRefId).NotEmpty()
                                .WithMessage("CustomerRefId is required.");
            RuleFor(command => command.Description).NotNull()
                                .WithMessage("Description is required.");
        }
    }
}