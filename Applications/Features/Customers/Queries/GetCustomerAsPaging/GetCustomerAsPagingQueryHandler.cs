using Applications.Features.Customers.Response;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Customers.Queries.GetCustomerAsPaging;

public sealed class GetCustomerAsPagingQueryHandler : IRequestHandler<GetCustomerAsPagingQuery, Pager<VCustomerList>>
{
    private readonly ICustomerService _customerService;
    public GetCustomerAsPagingQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<Pager<VCustomerList>> Handle(GetCustomerAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomersAsync(request.BranchId, request.UserId, request.CustomerId, request.Query,request.IsLoanActive, cancellationToken);
    }
}