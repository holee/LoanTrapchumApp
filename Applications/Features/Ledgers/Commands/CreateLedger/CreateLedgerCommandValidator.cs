namespace Applications.Features.Ledgers.Commands.CreateLedger;

public sealed class CreateLedgerCommandValidator : AbstractValidator<CreateLedgerCommand>
{
    public CreateLedgerCommandValidator()
    {
        RuleFor(x => x.Entity).NotNull().WithMessage("Ledger entity is required.");
        RuleFor(x => x.Entity.LedgerName).NotEmpty().WithMessage("Ledger name is required.");
        RuleFor(x => x.Entity.LedgerCode).NotEmpty().WithMessage("Ledger code is required.");
    }
}
