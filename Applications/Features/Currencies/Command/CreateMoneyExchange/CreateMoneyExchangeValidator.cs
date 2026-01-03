using Applications.Features.Currencies.Requests;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public sealed class CreateMoneyExchangeValidator : AbstractValidator<ExchangeDtoCreate>
{
    public CreateMoneyExchangeValidator()
    {
        RuleFor(x => x.BuyCurrencyId)
            .GreaterThan(0).WithMessage("Buy Currency is required.");
        RuleFor(x => x.SellCurrencyId)
            .GreaterThan(0).WithMessage("Sell Currency is required.");
        RuleFor(x => x.BuyAmount)
            .GreaterThan(0).WithMessage("Buy Amount must be greater than zero.");
        RuleFor(x => x.SellAmount)
            .GreaterThan(0).WithMessage("Sell Amount must be greater than zero.");
        RuleFor(x => x.ExchangeRate)
            .GreaterThan(0).WithMessage("Exchange Rate must be greater than zero.");

    }
}