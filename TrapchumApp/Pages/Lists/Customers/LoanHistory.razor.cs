using Applications.Features.Loans.Responses;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class LoanHistory
    {
        [Parameter] public string? CustomerId { get; set; }

        IEnumerable<VLoanDisbursments> VLoanDisbursements = [];

        async Task GetCustomerLoanHistory()
        {
            var rsp = await _customerService.GetCustomerLoanHistoryAsync(CustomerId);

            if (rsp.IsSuccess)
            {
                VLoanDisbursements = rsp.Data!;
            }
            else
            {
                VLoanDisbursements = [];
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetCustomerLoanHistory();

            await base.OnInitializedAsync();
        }
    }

}