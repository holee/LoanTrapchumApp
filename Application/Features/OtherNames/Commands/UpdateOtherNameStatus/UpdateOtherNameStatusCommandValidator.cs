namespace Application.Features.OtherNames.Commands.UpdateOtherNameStatus;

public sealed class UpdateOtherNameStatusCommandValidator : AbstractValidator<UpdateOtherNameStatusCommand>
{
    public UpdateOtherNameStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
