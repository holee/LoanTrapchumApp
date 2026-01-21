using Applications.Features.Currencies.Requests;
using Applications.Features.Transactions.ForiegnExchanges;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public sealed class EditMoneyExchangeValidator : AbstractValidator<ForeignExchangeDtoUpdate>
{
    public EditMoneyExchangeValidator()
    {
        RuleFor(x => x.BuyCurrencyRefId)
            .GreaterThan(0).WithMessage("Buy Currency is required.");
        RuleFor(x => x.SellCurrencyRefId)
            .GreaterThan(0).WithMessage("Sell Currency is required.");
        RuleFor(x => x.BuyAmount)
            .GreaterThan(0).WithMessage("Buy Amount must be greater than zero.");
        RuleFor(x => x.SellAmount)
            .GreaterThan(0).WithMessage("Sell Amount must be greater than zero.");
        RuleFor(x => x.ExchangeRate)
            .GreaterThan(0).WithMessage("Exchange Rate must be greater than zero.");

        When(x => x.Transaction is not null, () =>
        {

            RuleFor(x => x.Transaction!.TransactionDate)
                    .NotEmpty().WithMessage("Transaction Date is required.")
                    .Must(d => d >= DateTime.UtcNow.AddSeconds(-5)
                            && d <= DateTime.UtcNow.AddSeconds(5))
                    .WithMessage("Transaction Date must be the current time.");
        });



    }
}