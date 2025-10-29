using Applications.Features.Customers.Response;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.Customers.Queries.GetCustomerAsPaging
{
    public sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Pager<VCustomerList>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomersQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Pager<VCustomerList>> Handle(GetCustomersQuery request, CancellationToken cancellationToken = default)
        {
            return await _customerService.GetCustomersAsync(request.BranchId, request.UserId, request.CustomerId, request.Param);
        }
    }
}
