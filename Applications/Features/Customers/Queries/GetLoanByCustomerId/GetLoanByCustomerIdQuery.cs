namespace Applications.Features.Customers.Queries.GetLoanByCustomerId;

public sealed record GetLoanByCustomerIdQuery(string? CustomerId) : IRequest<Pager<VLoanDisbursments>>;
