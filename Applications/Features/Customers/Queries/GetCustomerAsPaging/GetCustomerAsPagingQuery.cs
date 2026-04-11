namespace Applications.Features.Customers.Queries.GetCustomerAsPaging;

public record GetCustomerAsPagingQuery(int? BranchId, string? UserId, string? CustomerId, QueryParam Query, bool isLoanActive = false) : IRequest<Pager<VCustomerList>>;
