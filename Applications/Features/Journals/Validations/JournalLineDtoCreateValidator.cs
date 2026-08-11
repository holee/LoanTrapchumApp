using FluentValidation;

namespace Applications.Features.Journals.Validations
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
