using Applications.Features.Customers.Dto;
using ErrorOr;

namespace Applications.Features.Customers.Queries.GetUpdateCustomerById; 

public record GetUpdateCustomerByIdQuery(string? CustomerId) : IRequest<ErrorOr<EntityCustomerCreate>>;
