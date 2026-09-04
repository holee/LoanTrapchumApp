namespace Application.Features.Ledgers.Queries.GetLedgerForUpdateById;

public class GetLedgerForUpdateByIdQueryValidator : AbstractValidator<GetLedgerForUpdateByIdQuery>
{
    public GetLedgerForUpdateByIdQueryValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotNull().WithMessage("Ledger Id is required.")
            .GreaterThan(0).WithMessage("Ledger Id must be greater than 0.");
    }
}
