using Applications.Features.Transactions.Journals;
using FluentValidation;

namespace Applications.Requests.Validations.Transactions
{
    public class JournalLineDtoCreateValidator : AbstractValidator<JournalLineDtoCreate> 
    {
        public JournalLineDtoCreateValidator()
        {
            RuleFor(x=>x.LedgerName).NotEmpty().WithMessage("Ledger name is required");
            RuleFor(x => x.LedgerRefId).NotEmpty().WithMessage("Ledger name is required.");
        }
    }
}
