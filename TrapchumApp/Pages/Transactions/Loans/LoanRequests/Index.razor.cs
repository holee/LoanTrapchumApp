using Applications.Enumerations;
using Applications.Extensions.Web;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class Index
    {

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        [CascadingParameter]
        public CascadingAppState BState { get; set; } = default!;

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
        private bool _isPageLoading = false; 
        void SelectedIndexChanged(int selectedIndex)
        {
            _selectedIndex = selectedIndex;
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            await BState.GetAuthenticatedUser(auth.User);

            await base.OnInitializedAsync();
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

            var query = new GetLoanRequestAsPagingQuery(_userId, BranchId, RequestStatus.All, param);

            var rsp = await mediator.Send(query);

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

           // var result = await DialogService.OpenAsync<LoanRequestModal>("Loan Request", param, options: options);
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