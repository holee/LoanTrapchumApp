using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Notes.Command.EditNote;

public sealed class EditNoteCommandHandler:IRequestHandler<EditNoteCommand,ErrorOr<int>>
{
    private readonly ICustomerNoteService noteService;

    public EditNoteCommandHandler(ICustomerNoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<ErrorOr<int>> Handle(EditNoteCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToEditNote();

        return await noteService.AddOrUpdateNote(model);
    }
}