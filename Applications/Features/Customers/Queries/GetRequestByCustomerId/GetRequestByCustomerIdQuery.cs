using Applications.Interfaces.Customers;

namespace Applications.Features.Customers.Queries.GetRequestByCustomerId;

public record GetRequestByCustomerIdQuery(string? CustomerId, int? BranchId = null) : IRequest<Pager<VLoanRequests>>;

public sealed class GetRequestByCustomerIdQueryHandler(ICustomerService cusService) : IRequestHandler<GetRequestByCustomerIdQuery, Pager<VLoanRequests>>
{
    public async Task<Pager<VLoanRequests>> Handle(GetRequestByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await cusService.GetCustomerLoanRequestAsync(request.CustomerId, cancellationToken);
    }
}
