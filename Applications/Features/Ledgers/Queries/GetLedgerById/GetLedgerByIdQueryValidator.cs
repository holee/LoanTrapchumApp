namespace Applications.Features.Ledgers.Queries.GetLedgerById;

public sealed class GetLedgerByIdQueryValidator : AbstractValidator<GetLedgerByIdQuery>
{
    public GetLedgerByIdQueryValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotNull().WithMessage("Ledger ID is required.")
            .GreaterThan(0).WithMessage("Ledger ID must be greater than 0.");
    }
}
