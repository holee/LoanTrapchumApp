namespace Applications.Features.Journals.Queries.GetJournalEntryDetailById;

public sealed class GetJournalEntryDetailByIdValidator : AbstractValidator<GetJournalEntryDetailByIdQuery>
{
    public GetJournalEntryDetailByIdValidator() 
    {
        RuleFor(x => x.JournalId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Journal ID is required.")
            .NotNull().WithMessage("Journal ID must be a valid GUID.");
    }
}
