namespace Application.Features.OtherNames.Commands.UpdateOtherName;

public sealed class UpdateOtherNameCommandValidator : AbstractValidator<UpdateOtherNameCommand>
{
    public UpdateOtherNameCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Dto.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.Dto.LastName)
            .NotEmpty().WithMessage("Last name is required.");
    }
}
