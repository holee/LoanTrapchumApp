using Applications.Features.Customers.Dto;
using Applications.Responses;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.Customers.CreateCustomer;

public record CreateCustomerCommand(EntityCustomerCreate Request) : IRequest<Result<string>>;
