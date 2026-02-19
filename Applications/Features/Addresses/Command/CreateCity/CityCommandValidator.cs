using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Command.CreateCity;

public sealed class CityCommandValidator : AbstractValidator<CityDtoCreate>
{
    public CityCommandValidator()
    {
        RuleFor(x => x.CityName).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("City name cannot be null.")
                    .NotEmpty()
                    .WithMessage("City name is required.");
    }
}
