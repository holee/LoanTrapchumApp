namespace Applications.Features.Currencies.Command.CreateCashVaultIn;

public sealed class CreateCashVaultInValidator : AbstractValidator<CreateCashVaultIn>
{
    public CreateCashVaultInValidator()
    {
        RuleFor(x => x.TrnxCashVault).SetValidator(new TrnxCashVaultDtoCreateValidator());
    }
}
