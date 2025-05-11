using Applications.Enumerations;
using Applications.Extensions;
using Applications.Responses.ViewModels.Customers;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerLoanRequest
    {
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }
        [Parameter, EditorRequired] public string? CustomerId { get; set; }

        protected IEnumerable<CustomerLoanRequestVm> _vLoanRequests = [];

        async Task GetCustomerLoanReuquests()
        {
            var rsp = await _customerService.GetCustomerLoanRequestAsync<CustomerLoanRequestVm>(BranchId, CustomerId);
            _vLoanRequests = rsp.Items ?? [];
        }

        // string loanRequestDetailsUrl = "";
        // string loanRequestUrl = "";
        // protected override void OnParametersSet()
        // {
        //     loanRequestDetailsUrl = $"";
        //     loanRequestUrl = $"/App/Loan/{BranchId}/Branches/Request/?UserId={UserId}&CustomerId={LoanRequest?.CustomerId}&LoanRequestId={LoanRequest?.LoanRequestId}";
        //     base.OnParametersSet();
        // }


        protected override async Task OnInitializedAsync()
        {
            if (CustomerId.IsNotNullOrEmpty())
            {
                await GetCustomerLoanReuquests();
            }

            await base.OnInitializedAsync();
        }

        protected void AddLoanRequest()
        {
            var uri = $"/App/Request/{BranchId}/Branches/{UserId}/CreateNewUser?CustomerId={CustomerId}&ActionMode={ActionMode.Create}";
            NavigationManager.NavigateTo(uri);
        }

    }
}