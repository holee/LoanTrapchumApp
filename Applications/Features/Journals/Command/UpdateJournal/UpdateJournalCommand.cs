using ErrorOr;
namespace Applications.Features.Journals.Command.UpdateJournal;

public record UpdateJournalCommand(JournalTransactionDtoUpdate req) : IRequest<ErrorOr<JournalEntryDto?>>; 
