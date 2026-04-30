namespace Applications.Features.Ledgers.Commands.DeleteLedger;

public sealed class DeleteLedgerCommandValidator : AbstractValidator<DeleteLedgerCommand>
{
    public DeleteLedgerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Ledger ID is required.")
            .GreaterThan(0).WithMessage("Ledger ID must be greater than 0.");
    }
}
