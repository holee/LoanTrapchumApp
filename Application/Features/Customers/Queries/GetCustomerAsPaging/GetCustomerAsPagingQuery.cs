namespace Application.Features.Customers.Queries.GetCustomerAsPaging;

public record GetCustomerAsPagingQuery(int? BranchId, string? UserId, string? CustomerId, QueryParam Query, bool IsLoanActive = false) : IRequest<Pager<VCustomerList>>;
