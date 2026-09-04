using Application.Features.Customers.Interfaces;
using Application.Features.Customers.Response;
using Application.Interfaces.Customers;
using Application.Responses.Paginations;

namespace Application.Features.Customers.UseCases
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
