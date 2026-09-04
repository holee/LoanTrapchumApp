namespace Application.Features.CashManagements.Command.CreateCashVaultIn;

public sealed class CreateCashVaultInValidator : AbstractValidator<StartCashVaultIn>
{
    public CreateCashVaultInValidator()
    {
        RuleFor(x => x.TrnxCashVault).SetValidator(new TrnxCashVaultDtoCreateValidator());
    }
}
