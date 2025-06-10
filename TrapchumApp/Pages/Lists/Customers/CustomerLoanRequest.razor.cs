using Applications.Extensions;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerLoanRequest
    {
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }
        [Parameter, EditorRequired] public string? CustomerId { get; set; }

        protected IEnumerable<VLoanRequests> _vLoanRequests = [];

        async Task GetCustomerLoanReuquests()
        {
            var rsp = await _customerService.GetCustomerLoanRequestAsync(BranchId, CustomerId,false);

            _vLoanRequests = rsp.Data ?? [];
        }


        protected override async Task OnInitializedAsync()
        {
            if (CustomerId.IsNotNullOrEmpty())
            {
                await GetCustomerLoanReuquests();
            }

            await base.OnInitializedAsync();
        }

        protected async Task AddLoanRequest()
        {

            if (await _loanRequestService.IsCustomerBusy(CustomerId))
            {
                NotificationService.Notify(NotificationSeverity.Error, "This customer cannot request loan again.");
               
                return;
            }

            var uri = $"/App/Request/{BranchId}/Branches/{UserId}/CreateNew?CustomerId={CustomerId}";

            NavigationManager.NavigateTo(uri);

        }

    }
}