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

        private decimal? _income = 0;
        private decimal? _expense = 0;
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

                Customer.Photo ??= "/assets/profiles/profile.png";

                _income = Customer.Income ?? 0;

                _expense = Customer.Expenditure ?? 0;

            }
        }



    }
}