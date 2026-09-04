using FluentValidation;

namespace Application.Features.Locations.Command.CreateLocation;

public sealed class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.Request.LocationName)
            .NotEmpty().WithMessage("Location name is required.")
            .MaximumLength(250).WithMessage("Location name must not exceed 250 characters.");

        RuleFor(x => x.Request.Latitude)
            .MaximumLength(250).WithMessage("Latitude must not exceed 250 characters.");

        RuleFor(x => x.Request.Longitude)
            .MaximumLength(250).WithMessage("Longitude must not exceed 250 characters.");
    }
}
