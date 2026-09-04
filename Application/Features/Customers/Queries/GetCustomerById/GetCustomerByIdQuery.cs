using Application.Responses;
using Application.Utils;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Application.Features.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdQuery(string? CustomerId) : IRequest<ErrorOr<EntityNames>>;
}
