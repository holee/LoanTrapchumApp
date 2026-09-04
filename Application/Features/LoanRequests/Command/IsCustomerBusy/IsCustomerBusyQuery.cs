using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.IsCustomerBusy;

public record IsCustomerBusyQuery(string? CustomerId) : IRequest<ErrorOr<bool>>;
