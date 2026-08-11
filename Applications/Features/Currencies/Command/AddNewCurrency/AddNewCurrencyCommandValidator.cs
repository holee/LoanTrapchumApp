namespace Applications.Features.Currencies.Command.AddNewCurrency;

public sealed class AddNewCurrencyCommandValidator : AbstractValidator<AddNewCurrencyCommand>
{
    public AddNewCurrencyCommandValidator()
    {
        RuleFor(x => x.Currency.CurrencyCode).NotEmpty().WithMessage("Currency code is required.");
        RuleFor(x => x.Currency.SpotRate).NotEmpty().WithMessage("Spot rate is required.");
        RuleFor(x => x.Currency.ExchangeRate).GreaterThan(0).WithMessage("Exchange rate must be greater than zero.");
    }
}
