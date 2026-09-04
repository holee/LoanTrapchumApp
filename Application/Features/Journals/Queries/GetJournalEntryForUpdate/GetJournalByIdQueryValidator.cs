namespace Application.Features.Journals.Queries.GetJournalEntryForUpdate;

public sealed class GetJournalEntryForUpdateByIdQueryValidator : AbstractValidator<GetJournalEntryForUpdateByIdQuery>
{
    public GetJournalEntryForUpdateByIdQueryValidator()
    {
        RuleFor(x => x.TransactionId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Transaction ID is required.")
            .NotNull()
                .WithMessage("Transaction ID must be a valid GUID.");
    }
}
