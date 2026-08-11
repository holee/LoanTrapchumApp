namespace Applications.Features.Journals.Queries.GetJournalForEdit;

public sealed class GetJournalForEditByIdQueryValidator : AbstractValidator<GetJournalForEditByIdQuery>
{
    public GetJournalForEditByIdQueryValidator()
    {
        RuleFor(x => x.TranxId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Journal ID is required.")
            .NotNull().WithMessage("Journal ID must be a valid GUID.");
    }
}
