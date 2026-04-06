using Applications.Features.Customers.Dto;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.EditCustomer;

public record EditCustomerCommand(EntityCustomerCreate Request) : IRequest<ErrorOr<VCustomerList>>;
