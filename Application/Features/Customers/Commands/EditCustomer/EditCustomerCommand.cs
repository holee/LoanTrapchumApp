using Application.Features.Customers.Dto;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Customers.Commands.EditCustomer;

public record EditCustomerCommand(EntityCustomerCreate Request) : IRequest<ErrorOr<VCustomerList>>;
