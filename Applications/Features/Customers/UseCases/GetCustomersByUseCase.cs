using Applications.Features.Customers.Interfaces;
using Applications.Features.Customers.Response;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomersByUseCase : IGetCustomersByUseCase
    {
        private readonly ICustomerService _customerService;

        public GetCustomersByUseCase(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        //public async Task<Pager<VCustomerList>> ExecuteAsync(int? branchId, string? userId, string? customerId, QueryParam param)
        //{
        //    return await _customerService.GetCustomersAsync(branchId, userId, customerId, param);
        //}

    }
}
