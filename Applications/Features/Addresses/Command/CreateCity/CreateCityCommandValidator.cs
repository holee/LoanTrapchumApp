namespace Applications.Features.Addresses.Command.CreateCity;

public sealed class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.City).SetValidator(new CityCommandValidator());
    }
}
