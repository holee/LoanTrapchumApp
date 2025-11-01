using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Notes.Queries.GetNoteById
{
    public sealed class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, ErrorOr<VCustomerNotes>>
    {
        private readonly ICustomerNoteService noteService;

        public GetNoteByIdQueryHandler(ICustomerNoteService noteService)
        {
            this.noteService = noteService;
        }

        public async Task<ErrorOr<VCustomerNotes>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken = default)
        {
            return await noteService.GetNoteById(request.NoteId, request.CustomerId, cancellationToken);
        }
    }
}
