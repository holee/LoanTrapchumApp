using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomersCommand
    {
        private readonly ICustomerService _customerService;

        public GetCustomersCommand(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Pager<VCustomerList>> ExecuteAsync(string? customerId, QueryParam param)
        {
            return await _customerService.GetCustomersAsync(customerId, param);
        }

    }
}
