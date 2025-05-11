using Applications.Extensions;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestDetails
    {
        
        [Parameter] public string? LoanRequestId { get; set; }
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public VLoanRequests? Data { get; set; } = null;
        [Parameter] public VCustomerList? Customer { get; set; } = null;
        protected override async Task OnParametersSetAsync()
        {
            if (Data is null && CustomerId.IsNotNullOrEmpty() && BranchId > 0 && LoanRequestId.IsNotNullOrEmpty())
            {
                var rsp = await _loanRequestService.GetLoanRequestAsync(BranchId, CustomerId, LoanRequestId, true);

                if (rsp.IsSuccess)
                {
                    Data = rsp.Data;
                }
            }
            await GetCustomerLoanReuqest(CustomerId);

            await base.OnParametersSetAsync();
        }


        async Task GetCustomerLoanReuqest(string? customerId)
        {
            if (customerId.IsNotNullOrEmpty() && Customer is null)
            {
                var resp = await _customerService.GetCustomerForRequestLoanAsync(customerId);

                if (resp.IsSuccess)
                {
                    Customer = resp.Data!;
                }
            }
        }

    }
}