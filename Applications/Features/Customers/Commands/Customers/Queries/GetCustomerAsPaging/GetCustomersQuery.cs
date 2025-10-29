using Applications.Features.Customers.Response;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.Customers.Queries.GetCustomerAsPaging
{

    public record GetCustomersQuery(int? BranchId, string? UserId, string? CustomerId, QueryParam Param)
        : IRequest<Pager<VCustomerList>>;
}
