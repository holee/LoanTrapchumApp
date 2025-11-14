using Applications.Features.Customers.Response;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Customers.Queries.GetCustomerAsPaging;

public record GetCustomerAsPagingQuery(int? BranchId, string? UserId, string? CustomerId, QueryParam Query) : IRequest<Pager<VCustomerList>>;
