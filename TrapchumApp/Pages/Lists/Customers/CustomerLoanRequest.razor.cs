using Applications.Extensions;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Features.LoanRequests.Command.IsCustomerBusy;
using Applications.Responses.ViewModels.Loans.Requests;
namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerLoanRequest
    {
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }
        [Parameter, EditorRequired] public string? CustomerId { get; set; }
        [CascadingParameter] Task<AuthenticationState> Auth { get; set; } = default!; 

        protected IEnumerable<VLoanRequests> _vLoanRequests = [];
        bool IsAccountOnly = false;
        bool IsAdminOnly = false;
        bool IsCoOnly = true;
        bool IsLoanCanDisburse = false;
        bool IsCanRequest = false;
        bool IsLoanCanReview = false;
        async Task GetCustomerLoanReuquests()
        {

            var rsp = await _customerService.GetCustomerLoanRequestAsync(BranchId, CustomerId,false);

            _vLoanRequests = rsp.Data ?? [];
        }


        protected override async Task OnInitializedAsync()
        {

            var auth = await Auth;

            IsCanRequest = await _authorizeService.MustHasPermissionAsync(auth.User, RequestsPolicies.Request);

            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AccountOnly);

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AdminOnly);


            if (CustomerId.IsNotNullOrEmpty())
            {
                await GetCustomerLoanReuquests();
            }

            await base.OnInitializedAsync();
        }

        protected async Task AddLoanRequest()
        {
            var query = new IsCustomerBusyQuery(CustomerId);

            var rsp = await mediator.Send(query);

            if (rsp.IsError == false && rsp.Value)
            {
                NotificationService.Notify(NotificationSeverity.Error, "This customer cannot request loan again.");
               
                return;
            }

            var uri = $"app/loan/request/{BranchId}/Branches/CreateNew/{CustomerId}/customer";

            NavigationManager.NavigateTo(uri);

        }

    }
}