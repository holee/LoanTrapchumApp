using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Notes.Queries.GetNoteById;

public sealed record GetNoteByIdQuery(int? NoteId, string? CustomerId) : IRequest<ErrorOr<VCustomerNotes>>;
