using ErrorOr;
namespace Application.Features.Journals.Command.CreateJournal;

public record CreateJournalCommand(JournalTransactionDtoCreate req) : IRequest<ErrorOr<JournalEntry?>>;
