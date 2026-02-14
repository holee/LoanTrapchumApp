namespace Applications.Features.Journals.Queries.GetJournalById;

public sealed class GetJournalByIdQueryValidator : AbstractValidator<GetJournalByIdQuery>
{
    public GetJournalByIdQueryValidator()
    {
        RuleFor(x => x.JournalId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Journal ID is required.")
            .NotNull().WithMessage("Journal ID must be a valid GUID.");
    }
}
