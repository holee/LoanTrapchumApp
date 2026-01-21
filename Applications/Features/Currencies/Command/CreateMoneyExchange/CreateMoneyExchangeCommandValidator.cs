using Applications.Features.Transactions.ForiegnExchanges;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public sealed class CreateMoneyExchangeCommandValidator : AbstractValidator<CreateMoneyExchangeCommand>
{
    public CreateMoneyExchangeCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new CreateMoneyExchangeValidator());
    }
}
