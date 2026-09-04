namespace Application.Features.Ledgers.Queries.GetLedgerDetailsById;

public sealed class GetLedgerDetailsByIdQueryValidator : AbstractValidator<GetLedgerDetailsByIdQuery>
{
    public GetLedgerDetailsByIdQueryValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotNull().WithMessage("Ledger ID is required.")
            .GreaterThan(0).WithMessage("Ledger ID must be greater than 0.");
    }
}
