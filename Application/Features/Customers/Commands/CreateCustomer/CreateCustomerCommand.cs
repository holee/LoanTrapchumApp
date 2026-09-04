using Application.Features.Customers.Dto;
using ErrorOr;

namespace Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(EntityCustomerCreate Request) : IRequest<ErrorOr<VCustomerList>>;
