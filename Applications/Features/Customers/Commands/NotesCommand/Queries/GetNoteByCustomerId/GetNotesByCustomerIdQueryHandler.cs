using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteByCustomerId;

public sealed class GetNotesByCustomerIdQueryHandler : IRequestHandler<GetNotesByCustomerIdQuery,ErrorOr<IEnumerable<VCustomerNotes>>>
{
    private readonly ICustomerNoteService noteService;

    public GetNotesByCustomerIdQueryHandler(ICustomerNoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<ErrorOr<IEnumerable<VCustomerNotes>>> Handle(GetNotesByCustomerIdQuery request, CancellationToken cancellationToken = default)
    {
        return await noteService.GetNotes(request.CustomerId, cancellationToken);
    }
}