using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteById;

public sealed record GetNoteByIdQuery(int? NoteId, string? CustomerId) : IRequest<ErrorOr<VCustomerNotes>>;
