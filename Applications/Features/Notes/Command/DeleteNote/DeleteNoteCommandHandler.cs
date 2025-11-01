using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Notes.Command.DeleteNote;

public sealed class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, ErrorOr<bool>>
{
    private readonly ICustomerNoteService noteService;

    public DeleteNoteCommandHandler(ICustomerNoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken = default)
    {
        return await noteService.DeleteNote(request.NoteId, request.CustomerId, cancellationToken);
    }
}
