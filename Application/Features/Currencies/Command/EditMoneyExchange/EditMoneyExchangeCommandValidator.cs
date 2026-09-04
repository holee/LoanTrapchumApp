namespace Application.Features.Currencies.Command.EditMoneyExchange;

public sealed class EditMoneyExchangeCommandValidator : AbstractValidator<EditMoneyExchangeCommand>
{
    public EditMoneyExchangeCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new EditMoneyExchangeValidator());
    }
}
