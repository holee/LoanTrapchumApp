using ErrorOr;

namespace Applications.Features.Customers.Commands.DeactivateCustomer;

public record DeactivateCustomerCommand(string? CustomerId):IRequest<ErrorOr<bool>>;
