namespace Application.Features.Currencies.Command.SetSpotRates;

public sealed class SetSpotRatesCommandValidator : AbstractValidator<SetSpotRatesCommand>
{

    public SetSpotRatesCommandValidator()
    {
        RuleFor(x => x.SpotRates).NotNull()
            .WithMessage("សូមបញ្ចូលបញ្ជីអត្រាប្តូរប្រាក់");
        RuleForEach(x => x.SpotRates).SetValidator(new SetSpotRateValidator());
    }

}
