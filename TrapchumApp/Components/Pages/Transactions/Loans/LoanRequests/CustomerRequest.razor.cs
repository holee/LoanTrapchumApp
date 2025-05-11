using Applications.Extensions;
using Applications.Responses.ViewModels.Customers;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class CustomerRequest
    {
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public VCustomerList? Customer { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            if (CustomerId.IsNotNullOrEmpty())
            {
                await GetCustomerLoanReuqest(CustomerId);
            }
            await base.OnInitializedAsync();
        }




        async Task GetCustomerLoanReuqest(string? customerId)
        {
            var resp = await _customerService.GetCustomerForRequestLoanAsync(customerId);

            if (resp.IsSuccess)
            {
                Customer = resp.Data!;
            }
        }



    }
}