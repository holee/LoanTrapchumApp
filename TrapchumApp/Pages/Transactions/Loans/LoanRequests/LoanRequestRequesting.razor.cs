using Applications.Enumerations;
using Applications.Extensions.Identity.Policies;
using Applications.Extensions.Web;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestRequesting
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; }
        private string? _userId { get; set; }
        private bool CanRequestLoan = false;
        private bool CanDisburseLoan = false;
        private bool CanAprovedLoan = false;
        private IEnumerable<VLoanRequests> LoanRequests = new List<VLoanRequests>();
        private int counts = 0;
        private string uri = "";
        private string filtertext = "";
        private string sortColumn = "RequestDate";
        private string createRequestUrl = "";
        private string loanRequestDetailsUrl = "";
        private int pageIndex = 0;
        private int pageSize = 10;
        private bool sortDescending = false;
        private bool isIncludeInactive = false;
        private bool isLoading = false;
        private int statusId = 1;
        private ClaimsPrincipal? _user;
        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthenticationState;

            _user = auth.User;

            _userId = auth.User.GetId();

            CanRequestLoan = (await _authorizeService.AuthorizeAsync(_user, Policies.CanCreate())).Succeeded;

            CanDisburseLoan = (await _authorizeService.AuthorizeAsync(_user, Policies.CanDisburseLoan())).Succeeded;

            CanAprovedLoan = (await _authorizeService.AuthorizeAsync(_user, Policies.CanAproveRequest())).Succeeded;

            await GetLoanRequestAsync(_userId);

            await base.OnInitializedAsync();
        }

        async Task LoadLoanData(LoadDataArgs args)
        {
            pageIndex = args.Skip!.Value;

            filtertext = args.Filter ?? string.Empty;

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortColumn = args.OrderBy.Split(" ")[0];

                string dir = args.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }

            await GetLoanRequestAsync(_userId);

        }


        async Task GetLoanRequestAsync(string? userId)
        {
            if (CanDisburseLoan)
            {
                await GetApproveLoanRequestAsync(null);
            }
            else
            {
                await GetRequestedLoansAsync(userId);
            }
        }






        async Task GetCreatedLoansAsync(string? userId)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);


            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, RequestStatus.New, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }


        async Task GetRequestedLoansAsync(string? userId)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);


            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, RequestStatus.New, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }


        async Task GetCompletedLoanRequestAsync(string? userId)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, RequestStatus.Completed, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;


            isLoading = false;

            StateHasChanged();
        }


        async Task GetApproveLoanRequestAsync(string? userId)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, RequestStatus.Approved, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }


        async Task GetVerrfiedLoanRequestAsync(string? userId)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, RequestStatus.New, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;


            isLoading = false;

            StateHasChanged();
        }




        async void PageChanged(int size)
        {
            pageSize = size;
            pageIndex = 0;
            await GetLoanRequestAsync(_userId);

        }

        async void FilterTextChanged(string? searchText = null)
        {
            filtertext = searchText ?? "";
            await GetLoanRequestAsync(_userId);
        }

        void RowSelectedChanged() { }


        void LoanRequestChecked(VLoanRequests param)
        {
            var baseURL = $"/Loan/Request/Review/{param.LoanRequestId}";
            var newUri = new Dictionary<string, object?>
        {
            {"BranchId",param.BranchId},
            {"CustomerId",param.CustomerId}
        }.AddQueryString(baseURL);

            NavigationManager.NavigateTo(newUri);
        }

        async Task VerifiedLoanRequest(VLoanRequests req)
        {
            var options = new DialogOptions
            {

            };
            var param = new Dictionary<string, object?>
        {
            {"CustomerId",req.CustomerId},
            {"RequestUserId",req.CreditAgentId},
            {"BranchId",req.BranchId},
            {"Amount",req.RequestAmount},
            {"LoanRequestId",req.LoanRequestId}
        };

            var result = await DialogService.OpenAsync<LoanRequestModal>("Loan Request", param, options: options);
        }


    }
}


//createRequestUrl = new Dictionary<string, object?>{
//    {"BranchId",BranchId},
//    {"UserId",UserId},
//    { "ActionMode",ActionMode.Create}
// }.AddQueryString($"/Loan/Customer/Request");

