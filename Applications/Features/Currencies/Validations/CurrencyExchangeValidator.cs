using Applications.Features.Currencies;
using FluentValidation;

namespace Applications.Requests.Validations.Currencies   
{
    public sealed class CurrencyExchangeValidator : AbstractValidator<ExchangeDtoCreate>
    {
        public CurrencyExchangeValidator()
        {
            RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("Exchange Rate must be greater than Zero.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than Zero.");
            RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Total Amount must be greater than Zero.");
        }
    }
}
