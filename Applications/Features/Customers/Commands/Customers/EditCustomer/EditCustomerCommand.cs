using Applications.Features.Customers.Dto;
using Applications.Responses;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.Customers.EditCustomer;

public record EditCustomerCommand(EntityCustomerCreate Request) : IRequest<Result<string>>;
