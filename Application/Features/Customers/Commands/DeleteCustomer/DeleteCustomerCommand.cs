namespace Application.Features.Customers.Commands.DeleteCustomer;

public sealed record DeleteCustomerCommand(string? CustomerId) : IRequest<DeleteResult>;
