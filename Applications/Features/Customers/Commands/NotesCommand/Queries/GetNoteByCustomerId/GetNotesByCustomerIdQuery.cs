using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteByCustomerId;

public sealed record GetNotesByCustomerIdQuery(string? CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerNotes>>>;
