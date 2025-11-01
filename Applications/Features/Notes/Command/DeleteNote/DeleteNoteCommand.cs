using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Notes.Command.DeleteNote;

public sealed record DeleteNoteCommand(int? NoteId, string? CustomerId) : IRequest<ErrorOr<bool>>;
