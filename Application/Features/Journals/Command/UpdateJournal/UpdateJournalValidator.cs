namespace Application.Features.Journals.Command.UpdateJournal;

public sealed class UpdateJournalValidator : AbstractValidator<JournalTransactionDtoUpdate>
{
    public UpdateJournalValidator() 
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
