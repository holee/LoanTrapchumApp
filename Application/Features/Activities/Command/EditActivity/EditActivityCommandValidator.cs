using Application.Features.Activities.Command.CreateActivity;
using FluentValidation;

namespace Application.Features.Activities.Command.EditActivity
{
    public sealed class EditActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public EditActivityCommandValidator()
        {
            RuleFor(command => command.Request.ActivityDate).NotNull()
                                .WithMessage("Activity Date is required.");
            RuleFor(x=>x.Request.CustomerRefId).NotEmpty()
                                .WithMessage("CustomerRefId is required.");
            RuleFor(command => command.Request.Description).NotNull()
                                .WithMessage("Description is required.");
        }
    }
}