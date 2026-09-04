namespace Application.Features.Journals.Command.CreateJournal;

public sealed class CreateJournalCommandValidator : AbstractValidator<CreateJournalCommand>
{
    public CreateJournalCommandValidator()
    {
        RuleFor(x => x.req).SetValidator(new CreateJournalValidator());
    }
}
