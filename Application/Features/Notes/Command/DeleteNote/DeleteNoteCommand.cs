using Application.Utils;
using ErrorOr;

namespace Application.Features.Notes.Command.DeleteNote;

public sealed record DeleteNoteCommand(int? NoteId, string? CustomerId) : IRequest<ErrorOr<bool>>;
