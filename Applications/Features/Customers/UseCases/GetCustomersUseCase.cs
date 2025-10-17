using Applications.Features.Customers.Response;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomersUseCase
    {
        private readonly ICustomerService _customerService;

        public GetCustomersUseCase(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Pager<VCustomerList>> ExecuteAsync(int? branchId, string? userId, string? customerId, QueryParam param)
        {
            return await _customerService.GetCustomersAsync(branchId, userId, customerId, param);
        }

    }
}
