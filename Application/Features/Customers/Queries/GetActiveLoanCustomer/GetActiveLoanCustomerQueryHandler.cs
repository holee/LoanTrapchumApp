using Application.Interfaces.Customers;

namespace Application.Features.Customers.Queries.GetActiveLoanCustomer;

public sealed class GetActiveLoanCustomerQueryHandler : IRequestHandler<GetActiveLoanCustomerQuery, Pager<VCustomerList>>
{
    private readonly ICustomerService _customerService;
    public GetActiveLoanCustomerQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<Pager<VCustomerList>> Handle(GetActiveLoanCustomerQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomersAsync(request.BranchId, null, null, request.Query, true, cancellationToken);

    }
}