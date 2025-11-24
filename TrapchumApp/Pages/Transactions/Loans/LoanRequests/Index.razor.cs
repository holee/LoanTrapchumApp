using Applications.Enumerations;
using Applications.Extensions.Identity.Policies;
using Applications.Extensions.Web;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Authorization;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class Index
    {

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; } = 0;
        private string? _userId = string.Empty;
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
        bool CanRequestLoan = false;
        bool CanDisburseLoan = false;
        bool CanAprovedLoan = false;
        int status = 0;
        bool IsAccount = false;
        bool IsCreditAgent = false;
        bool IsManager = false;
        void SelectedIndexChanged(int selectedIndex)
        {


        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            IsAccount = (await _authorizeService.AuthorizeAsync(auth.User, Policies.IsAccountOnly())).Succeeded;
           
            IsCreditAgent = (await _authorizeService.AuthorizeAsync(auth.User, Policies.IsCreditOfficerOnly())).Succeeded;
           
            IsManager = (await _authorizeService.AuthorizeAsync(auth.User, Policies.IsManagerOnly())).Succeeded;

            CanRequestLoan = (await _authorizeService.AuthorizeAsync(auth.User, RequestsPolicies.CanCreate())).Succeeded;
            CanDisburseLoan = (await _authorizeService.AuthorizeAsync(auth.User, RequestsPolicies.CanDisburseLoan())).Succeeded;
            CanAprovedLoan = (await _authorizeService.AuthorizeAsync(auth.User, RequestsPolicies.CanAproveRequest())).Succeeded;

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            createRequestUrl = new Dictionary<string, object?>
        {
            {"BranchId",BranchId},
            {"UserId",_userId},
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
            await GetLoanRequestAsync();
        }



        async Task GetLoanRequestAsync()
        {
            isLoading = true;
            string? aproveUserId = string.Empty, creditAgentId = string.Empty;
            //int statusId = 0;
            if (CanRequestLoan)
            {
                creditAgentId = _userId;
                //aproveUserId = 0;
            }
            if (CanAprovedLoan)
            {
                aproveUserId = _userId;
                // creditAgentId = 0;
            }

            if (CanDisburseLoan)
            {
                //creditAgentId = 0;
                //aproveUserId = 0;
                //statusId = 0;
            }

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

            var rsp = await _loanRequestService.GetLoanRequestsAsync(_userId, BranchId, RequestStatus.All, param);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }


        void GotoLoanRequest()
        {
            createRequestUrl = new Dictionary<string, object?>
                {
                    {"BranchId",BranchId},
                    {"UserId",_userId}
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


        void CheckLoanRequest(VLoanRequests req)
        {
            NavigationManager.NavigateTo($"/Loan/{req.CustomerId}/Request/{req.LoanRequestId}?BranchId={req.BranchId}&UserId={_userId}");
        }

        void LoanDisbursment(long? CustomerId, long? loanRequestId)
        {
            NavigationManager.NavigateTo($"/Loan/Disbursement/CreateNewUser?BranchId={BranchId}&UserId={_userId}&CustomerRequestId={CustomerId}&LoanRequestId={loanRequestId}");
        }


        BadgeStyle ChangedState(int? status)
        {
            return status switch
            {

                1 => BadgeStyle.Light,
                2 => BadgeStyle.Warning,
                3 => BadgeStyle.Info,
                4 => BadgeStyle.Success,
                5 => BadgeStyle.Primary,
                6 => BadgeStyle.Secondary,
                7 => BadgeStyle.Danger,
                _ => BadgeStyle.Dark
            };
        }


    }
}