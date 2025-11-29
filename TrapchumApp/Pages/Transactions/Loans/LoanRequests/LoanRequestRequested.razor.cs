using Applications.Enumerations;
using Applications.Extensions.Identity.Policies;
using Applications.Extensions.Web;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsWithCanDisburse;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using System.Security.Claims;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestRequested
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; }
        [Parameter] public string? UserId { get; set; }
        bool CanRequestLoan { get; set; } = false;
        bool CanDisburseLoan { get; set; } = false;
        bool CanAprovedLoan { get; set; } = false;
        IEnumerable<VLoanRequests> LoanRequests = [];

        int counts = 0;
        string uri = "";
        string filtertext = "";
        string sortColumn = "RequestDate";
        string createRequestUrl = "";
        string loanRequestDetailsUrl = @$"";
        int pageIndex = 0;
        int pageSize = 10;
        bool sortDescending = false;
        bool isIncludeInactive = false;
        bool isLoading = false;
        int statusId = 1;
        ClaimsPrincipal? _user;
        bool isAllowApproved = false;
        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthenticationState;

            _user = auth.User;

            CanRequestLoan = (await _authorizeService.AuthorizeAsync(_user, Policies.CanCreate())).Succeeded;

            CanDisburseLoan = (await _authorizeService.AuthorizeAsync(_user, Policies.CanDisburseLoan())).Succeeded;

            CanAprovedLoan = (await _authorizeService.AuthorizeAsync(_user, Policies.CanAproveRequest())).Succeeded;

            await GetLoanRequestAsync(UserId);

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            createRequestUrl = new Dictionary<string, object?>
        {
            {"BranchId",BranchId},
            {"UserId",UserId},
            { "ActionMode",ActionMode.Create}
        }.AddQueryString($"/Loan/Customer/Request");
            base.OnParametersSet();
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

            await GetLoanRequestAsync(UserId);

        }


        async Task GetLoanRequestAsync(string? userId)
        {
            await GetCreatedLoansAsync(userId);
        }

        async Task GetCreatedLoansAsync(string? userId)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

            var query = new GetLoanRequestsWithCanDisburseQuery(userId, false, BranchId, RequestStatus.Requested, param);

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
            await GetLoanRequestAsync(UserId);

        }

        async void FilterTextChanged(string? searchText = null)
        {
            filtertext = searchText ?? "";
            await GetLoanRequestAsync(UserId);
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




//async Task GetVerrfiedLoanRequestAsync(string? userId)
//{
//    isLoading = true;

//    var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

//    var rsp = await _loanRequestService.GetLoanRequestsAsync(userId, BranchId, RequestStatus.Verified, param);


//    LoanRequests = rsp.Items;

//    counts = rsp.Count;


//    isLoading = false;

//    StateHasChanged();
//}


//void GotoLoanRequest()
//{
//    createRequestUrl = new Dictionary<string, object?>
//{
//    {"BranchId",BranchId},
//    {"UserId",UserId}
//}.AddQueryString($"/Loan/0/Customer/0/Request");
//    NavigationManager.NavigateTo(createRequestUrl);
//}
//async Task GetRequestedLoansAsync(string? userId)
//{
//    isLoading = true;

//    var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

//    var rsp = await _loanRequestService.GetLoanRequestsAsync(userId, BranchId, RequestStatus.Requested, param);


//    LoanRequests = rsp.Items;

//    counts = rsp.Count;

//    isLoading = false;

//    StateHasChanged();
//}


//async Task GetCompletedLoanRequestAsync(string? userId)
//{
//    isLoading = true;

//    var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

//    var rsp = await _loanRequestService.GetLoanRequestsAsync(userId, BranchId, RequestStatus.Completed, param);


//    LoanRequests = rsp.Items;

//    counts = rsp.Count;


//    isLoading = false;

//    StateHasChanged();
//}


//async Task GetApproveLoanRequestAsync(string? userId)
//{
//    isLoading = true;

//    var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

//    var rsp = await _loanRequestService.GetLoanRequestsAsync(userId, BranchId, RequestStatus.Aprove, param);


//    LoanRequests = rsp.Items;

//    counts = rsp.Count;


//    isLoading = false;

//    StateHasChanged();
//}