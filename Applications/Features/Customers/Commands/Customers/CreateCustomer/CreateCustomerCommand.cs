using Applications.Features.Customers.Dto;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Customers.CreateCustomer;

public record CreateCustomerCommand(EntityCustomerCreate Request) : IRequest<ErrorOr<VCustomerList>>;
