using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Notes.Command.CreateNote;

public sealed class CreateNoteCommandHandler :IRequestHandler<AddNoteCommand,ErrorOr<int>>
{
    private readonly ICustomerNoteService noteService;

    public CreateNoteCommandHandler(ICustomerNoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<ErrorOr<int>> Handle(AddNoteCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToCreateNote();

        return await noteService.AddOrUpdateNote(model);
    }
}