using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Notes.Queries.GetNoteByCustomerId;

public sealed record GetNotesByCustomerIdQuery(string? CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerNotes>>>;
