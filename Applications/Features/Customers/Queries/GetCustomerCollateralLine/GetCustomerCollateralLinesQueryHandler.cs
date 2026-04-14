using Applications.Interfaces.Customers;

namespace Applications.Features.Customers.Queries.GetCustomerCollateralLine;

public sealed class GetCustomerCollateralLinesQueryHandler : IRequestHandler<GetCustomerCollateralLinesQuery, Pager<VCollateralLines>>
{
    private readonly ICustomerService _customerService;
    public GetCustomerCollateralLinesQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<Pager<VCollateralLines>> Handle(GetCustomerCollateralLinesQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomerCollateralLines(request.CustomerId, request.IncludeInUsed);
    }
}
