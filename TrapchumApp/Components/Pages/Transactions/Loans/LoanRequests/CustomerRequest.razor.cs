using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class CustomerRequest
    {
        private readonly ICustomerService _customerService;

        public CustomerRequest(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public VCustomerList? Customer { get; set; } = new();
        protected override async Task OnParametersSetAsync()
        {
            if (CustomerId.NotEmpty())
            {
                await GetCustomerLoanReuqest(CustomerId);
            }
            await base.OnParametersSetAsync();
        }
        async Task GetCustomerLoanReuqest(Guid? customerId)
        {
            var resp = await _customerService.GetCustomerForRequestLoanAsync(customerId);

            if (resp.IsSuccess)
            {
                Customer = resp.Data!;
            }
        }



    }
}