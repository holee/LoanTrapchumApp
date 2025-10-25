using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.NotesCommand.DeleteNoteCommand;

public sealed record DeleteNoteCommand(int? NoteId, string? CustomerId) : IRequest<ErrorOr<bool>>;
