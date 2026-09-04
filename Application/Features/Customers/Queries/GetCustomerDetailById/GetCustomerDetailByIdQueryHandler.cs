using Application.Interfaces.Customers;
using ErrorOr;

namespace Application.Features.Customers.Queries.GetCustomerDetailById;

public sealed class GetCustomerDetailByIdQueryHandler : IRequestHandler<GetCustomerDetailByIdQuery, ErrorOr<VCustomerDetails>>
{
    private readonly ICustomerService _customerService;
    public GetCustomerDetailByIdQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<ErrorOr<VCustomerDetails>> Handle(GetCustomerDetailByIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomerDetailByIdAsync(request.CustomerId, request.Include, cancellationToken);
    }
}
