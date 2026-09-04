namespace Application.Features.Ledgers.Commands.ChangeLedgerStatus;

public sealed class ChangeLedgerStatusCommandValidator : AbstractValidator<ChangeLedgerStatusCommand>
{
    public ChangeLedgerStatusCommandValidator()
    {
        RuleFor(x => x.Req.LedgerId)
            .GreaterThan(0).WithMessage("Ledger ID must be greater than 0.");
    }
}
