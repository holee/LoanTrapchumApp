using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.IsCustomerBusy;

public record IsCustomerBusyQuery(string? CustomerId) : IRequest<ErrorOr<bool>>;
