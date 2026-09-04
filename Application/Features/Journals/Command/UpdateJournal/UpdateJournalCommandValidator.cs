namespace Application.Features.Journals.Command.UpdateJournal;

public sealed class UpdateJournalCommandValidator : AbstractValidator<UpdateJournalCommand>
{
    public UpdateJournalCommandValidator()
    {
        RuleFor(x => x.req).SetValidator(new UpdateJournalValidator());
    }
}
