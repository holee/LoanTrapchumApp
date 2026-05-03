using Applications.Extensions;
using Applications.Features.Customers.Queries.GetCustomerDetailById;
using Applications.Features.Customers.Response;

namespace TrapchumApp.Pages.Transactions.LoanRequests
{
    public partial class CustomerRequest
    {
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public VCustomerDetails? Customer { get; set; } = new();
        [Parameter] public bool ShowAdditionalInfo { get; set; } = false;
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
            var resp = await mediator.Send(new GetCustomerDetailByIdQuery(customerId)); //_customerService.GetCustomerForRequestLoanAsync(customerId);

            if (resp.IsSuccess())
            {
                Customer = resp.Value;
            }
        }



    }
}