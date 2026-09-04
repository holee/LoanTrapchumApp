namespace Application.Features.Journals.Queries.GetJournalTransaction;

public sealed class GetJournalTransactionByIdQueryValidator : AbstractValidator<GetJournalTransactionByIdQuery>
{
    public GetJournalTransactionByIdQueryValidator()
    {
        RuleFor(x => x.TransactionId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Transaction ID is required.")
            .NotNull()
                .WithMessage("Transaction ID must be a valid GUID.");
    }
}
