namespace Application.Features.Addresses.Command.CreateCity;

public sealed class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.City).SetValidator(new CityCommandValidator());
    }
}
