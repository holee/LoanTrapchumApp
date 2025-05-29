using System.Security.Claims;
using Applications.Enumerations;
using Applications.Extensions.Identity.Policies;
using Applications.Extensions.Web;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestAll(ILoanRequestServiceImpl loanRequestService)
    {
        private readonly ILoanRequestServiceImpl _requestService = loanRequestService;

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] private CascadingAppState bState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; }
        [Parameter] public string? UserId { get; set; }
        bool CanRequestLoan { get; set; } = false;
        bool CanDisburseLoan { get; set; } = false;
        bool CanAprovedLoan { get; set; } = false;
        ClaimsPrincipal? _user;

        IEnumerable<VLoanRequests> LoanRequests = [];

        int counts = 0;
        string uri = "";
        string filtertext = "";
        string sortColumn = "RequestDate";
        string createRequestUrl = "";
        int pageIndex = 0;
        int pageSize = 10;
        bool sortDescending = false;
        bool isIncludeInactive = false;
        bool isLoading = false;
        int statusId = 1;

        protected override async Task OnInitializedAsync()
        {
            createRequestUrl = $"/App/Request/{BranchId}/Branches/{UserId}/CreateNew";
            var auth = await AuthenticationState;
            _user = auth.User;
            CanRequestLoan = (await _authorizeService.AuthorizeAsync(_user, RequestsPolicies.CanCreate()))
                .Succeeded;
            CanDisburseLoan = (await _authorizeService.AuthorizeAsync(_user, RequestsPolicies.CanDisburseLoan()))
                .Succeeded;
            CanAprovedLoan = (await _authorizeService.AuthorizeAsync(_user, RequestsPolicies.CanAproveRequest()))
                .Succeeded;

            await bState.CheckedCanDisbursemnt(_user);

            await GetLoanRequestAsync(UserId);

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

            await GetLoanRequestAsync();
        }

        async Task GetLoanRequestAsync(string? userId)
        {
            isLoading = true;


            var param = new QueryParam(pageIndex, pageSize, sortColumn,false, sortDescending, filtertext);

            if (bState.CanDisbursed)
            {
                userId = string.Empty;
            }
            var rsp = await _requestService.GetLoanRequestsAsync(userId, CanDisburseLoan, BranchId, RequestStatus.All, param);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }


        async Task GetLoanRequestAsync()
        {
            // if (CanDisburseLoan)
            // {
            await GetLoanRequestAsync(null);
            // }
            // else
            // {
            //     await GetLoanRequestAsync(UserId);
            // }
        }


        void GotoLoanRequest()
        {
            createRequestUrl = new Dictionary<string, object?>
            {
                { "BranchId", BranchId },
                { "UserId", UserId }
            }.AddQueryString($"/Loan/0/Customer/0/Request");
            NavigationManager.NavigateTo(createRequestUrl);
        }

        async void PageChanged(int size)
        {
            pageSize = size;
            await GetLoanRequestAsync();
        }

        async void FilterTextChanged(string? searchText = null)
        {
            filtertext = searchText ?? "";

            await GetLoanRequestAsync();
        }

        void RowSelectedChanged()
        {
        }


        void LoanRequestChecked(VLoanRequests param)
        {
            var baseURL = $"/Loan/Request/{param.LoanRequestId}/Review";
            var newUri = new Dictionary<string, object?>
            {
                { "BranchId", param.BranchId },
                { "CustomerId", param.CustomerId },
                { "UserId", UserId }
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
                { "CustomerId", req.CustomerId },
                { "RequestUserId", req.CreditAgentId },
                { "BranchId", req.BranchId },
                { "Amount", req.RequestAmount },
                { "LoanRequestId", req.LoanRequestId }
            };

            var result = await DialogService.OpenAsync<LoanRequestModal>("Loan Request", param, options: options);
        }
    }
}