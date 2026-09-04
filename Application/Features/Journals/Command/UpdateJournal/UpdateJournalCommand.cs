using ErrorOr;
namespace Application.Features.Journals.Command.UpdateJournal;

public record UpdateJournalCommand(JournalTransactionDtoUpdate req) : IRequest<ErrorOr<JournalEntryDto?>>; 
