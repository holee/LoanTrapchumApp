using Applications.Extensions;
using Applications.Features.Customers.Queries.GetRequestByCustomerId;
using Applications.Features.LoanRequests.Command.IsCustomerBusy;
using Applications.Responses.ViewModels.Loans.Requests;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerLoanRequest
    {
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter, EditorRequired] public string? CustomerId { get; set; }
        [CascadingParameter] Task<AuthenticationState> Auth { get; set; } = default!;

        protected IEnumerable<VLoanRequests> _vLoanRequests = [];

        private string? _userId { get; set; } = string.Empty;
       
        async Task GetCustomerLoanReuquests()
        {

            var rsp = await mediator.Send(new GetRequestByCustomerIdQuery(CustomerId, null));//_customerService.GetCustomerLoanRequestAsync(BranchId, CustomerId,false);

            _vLoanRequests = rsp.Items;
        }


        protected override async Task OnInitializedAsync()
        {

            var auth = await Auth;

            _userId = auth.GetUserId();

            await AppState.GetAuthenticatedUser(auth.User);

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

            var uri = $"/App/Loans/Requests/{BranchId}/Branches/CreateNew/{CustomerId}/Customers";

            NavigationManager.NavigateTo(uri);

        }

    }
}