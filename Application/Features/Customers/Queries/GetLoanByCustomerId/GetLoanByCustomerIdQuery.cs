using Application.Features.Loans.Dtos.Responses;

namespace Application.Features.Customers.Queries.GetLoanByCustomerId;

public sealed record GetLoanByCustomerIdQuery(string? CustomerId) : IRequest<Pager<VLoanDisbursments>>;
