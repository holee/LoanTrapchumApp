namespace Applications.Features.Ledgers.Commands.CreateOrUpdateLedger;

public sealed class CreateOrUpdateLedgerCommandValidator : AbstractValidator<CreateOrUpdateLedgerCommand>
{
    public CreateOrUpdateLedgerCommandValidator()
    {
        RuleFor(x => x.Entity).NotNull().WithMessage("Ledger entity is required.");
        RuleFor(x => x.Entity.LedgerName).NotEmpty().WithMessage("Ledger name is required.");
        RuleFor(x => x.Entity.LedgerCode).NotEmpty().WithMessage("Ledger code is required.");
    }
}
