namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public sealed class CreateMoneyExchangeCommandValidator : AbstractValidator<EditMoneyExchangeCommand>
{
    public CreateMoneyExchangeCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new CreateMoneyExchangeValidator());
    }
}
