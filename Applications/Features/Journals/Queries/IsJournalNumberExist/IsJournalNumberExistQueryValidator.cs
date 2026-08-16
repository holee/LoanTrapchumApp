namespace Applications.Features.Journals.Queries.IsJournalNumberExist;

public sealed class IsJournalNumberExistQueryValidator : AbstractValidator<IsJournalNumberExistQuery>
{
    public IsJournalNumberExistQueryValidator()
    {
        RuleFor(x => x.JournalNumber)
            .NotEmpty().WithMessage("Journal number is required.")
            .MaximumLength(50).WithMessage("Journal number cannot exceed 50 characters.");
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("Branch reference ID is required.")
            .GreaterThan(0).WithMessage("Branch reference ID must be greater than zero.");
    }
}
