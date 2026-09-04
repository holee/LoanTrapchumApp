using Application.Features.Currencies.Command.AddCurrencyNote;

namespace Application.Features.Currencies.Command.AddNote;

public sealed class AddNoteCommandValidator : AbstractValidator<AddCurrencyNoteCommand>
{
    public AddNoteCommandValidator()
    {
        RuleFor(x => x.CurrencyCounter).SetValidator(new CurrencyCounterDtoCreateValidator());
    }
}
