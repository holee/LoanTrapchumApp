using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Command.UpdateCity;

public sealed class CityUpdateCommandValidator : AbstractValidator<CityDtoUpdate>
{
    public CityUpdateCommandValidator()
    {

        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("City ID cannot be null.")
                    .GreaterThan(0).WithMessage("City ID must be greater than zero.");

        RuleFor(x => x.CityName).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("City name cannot be null.")
                    .NotEmpty()
                    .WithMessage("City name is required.");
    }
}
