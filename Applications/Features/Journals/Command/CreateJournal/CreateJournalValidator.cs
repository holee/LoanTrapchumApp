using Applications.Features.Transactions.Journals;

namespace Applications.Features.Journals.Command.CreateJournal;

public sealed class CreateJournalValidator : AbstractValidator<JournalTransactionDtoCreate>
{
    public CreateJournalValidator()
    {
        RuleFor(x => x.BranchRefId).Cascade(CascadeMode.Stop)
                   .NotNull().WithMessage("Branch reference ID is required.")
                   .GreaterThan(0).WithMessage("Branch reference ID must be greater than zero.");
        RuleFor(x => x.CurrencyRefId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Currency reference ID is required.")
                    .GreaterThan(0).WithMessage("Currency reference ID must be greater than zero.");
        RuleFor(x => x.TranTypeRefId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Transaction type reference ID is required.")
                    .GreaterThan(0).WithMessage("Transaction type reference ID must be greater than zero.");

        When(x => x.JournalEntry != null, () =>
        {
           
        });
    }

}
