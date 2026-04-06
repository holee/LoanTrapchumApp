using Applications.Responses;
using Applications.Utils;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdQuery(string? CustomerId) : IRequest<ErrorOr<EntityNames>>;
}
