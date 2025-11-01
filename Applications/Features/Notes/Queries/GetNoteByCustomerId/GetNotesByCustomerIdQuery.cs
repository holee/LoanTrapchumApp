using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Notes.Queries.GetNoteByCustomerId;

public sealed record GetNotesByCustomerIdQuery(string? CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerNotes>>>;
