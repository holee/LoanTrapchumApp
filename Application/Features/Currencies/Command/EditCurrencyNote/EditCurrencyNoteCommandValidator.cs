using Application.Features.Currencies.Command.AddNote;

namespace Application.Features.Currencies.Command.EditCurrencyNote;

public sealed class EditCurrencyNoteCommandValidator : AbstractValidator<EditCurrencyNoteCommand>
{
    public EditCurrencyNoteCommandValidator()
    {
        RuleFor(x => x.CurrencyCounter).SetValidator(new CurrencyCounterDtoUpdateValidator());
    }
}
