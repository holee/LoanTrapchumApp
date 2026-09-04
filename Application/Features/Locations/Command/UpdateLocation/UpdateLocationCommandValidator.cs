using FluentValidation;

namespace Application.Features.Locations.Command.UpdateLocation;

public sealed class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .GreaterThan(0).WithMessage("A valid location Id is required for update.");

        RuleFor(x => x.Request.LocationName)
            .NotEmpty().WithMessage("Location name is required.")
            .MaximumLength(250).WithMessage("Location name must not exceed 250 characters.");

        RuleFor(x => x.Request.Latitude)
            .MaximumLength(250).WithMessage("Latitude must not exceed 250 characters.");

        RuleFor(x => x.Request.Longitude)
            .MaximumLength(250).WithMessage("Longitude must not exceed 250 characters.");
    }
}
