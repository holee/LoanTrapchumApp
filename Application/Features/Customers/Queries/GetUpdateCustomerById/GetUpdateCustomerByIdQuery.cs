using Application.Features.Customers.Dto;
using ErrorOr;

namespace Application.Features.Customers.Queries.GetUpdateCustomerById; 

public record GetUpdateCustomerByIdQuery(string? CustomerId) : IRequest<ErrorOr<EntityCustomerCreate>>;
