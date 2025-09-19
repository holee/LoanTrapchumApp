using Applications.Features.Transactions.Journals;
using FluentValidation;
namespace Applications.Validations.Transactions
{
    public class JournalDtoCreateValidator : AbstractValidator<JournalDtoCreate>
    {
        public JournalDtoCreateValidator()
        {
            RuleFor(x => x.JournalType).NotEmpty().WithMessage("Journal type is required.");
            RuleFor(x => x.ReferrenceNum).NotEmpty().WithMessage("Referrence number is required.");
            RuleFor(x => x.BankRefId).NotEmpty().WithMessage("Bank number is required.")
                                     .GreaterThan(0).WithMessage("Bank number is required.");
            RuleFor(x => x.CurrencyRefId).NotEmpty().WithMessage("Currency is required.")
                                     .GreaterThan(0).WithMessage("Currency is required.");
        }
    }
}