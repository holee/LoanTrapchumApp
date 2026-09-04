using ErrorOr;

namespace Application.Features.Customers.Commands.DeactivateCustomer;

public record DeactivateCustomerCommand(string? CustomerId):IRequest<ErrorOr<bool>>;
