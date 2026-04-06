namespace Applications.Features.Customers.Queries.GetActiveLoanCustomer;

public record GetActiveLoanCustomerQuery(int? BranchId, QueryParam Query) : IRequest<Pager<VCustomerList>>;
