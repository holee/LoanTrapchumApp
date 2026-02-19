namespace Applications.Features.Addresses.Command.UpdateCity;

public sealed class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.City).SetValidator(new CityUpdateCommandValidator());
    }
}
