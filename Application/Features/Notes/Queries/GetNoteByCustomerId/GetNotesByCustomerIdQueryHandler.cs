using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Notes.Queries.GetNoteByCustomerId;

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