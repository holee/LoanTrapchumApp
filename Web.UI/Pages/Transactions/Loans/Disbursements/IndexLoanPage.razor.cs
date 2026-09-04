using Applications.Features.Loans.Dtos.Responses;

namespace Web.UI.Pages.Transactions.Loans.Disbursements
{
    public partial class IndexLoanPage
    {
        [Parameter] public int BranchId { get; set; } = 0;
   
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        bool branchStatus = false;
        int counts = 0;
        int loanCount = 0;
        int pageSize = 10;
        bool isLoading = false;
        bool isProgess = false;
        bool isAdmin = false;
        bool isNewLoan = false;
        bool isScheduleTab = false;
        bool includeInactive = false;
        int selectedTabIndex = 0;
        string sortField = "RegisterDate";
        private string? _userId;
        bool sortDescending = true;
        int pageIndex = 0;



        protected override async Task OnInitializedAsync()
        {
            _userId = (await AuthenticationState).GetUserId();  

            await base.OnInitializedAsync();
        }
        void DownLoadScheduleAsExcel(string? loanRefId, int? branchId)
        {
            ContextMenuService.Close();
            isProgess = true;
            isProgess = false;
            StateHasChanged();
        }



        void DownLoadScheduleAsPdf(string? loanRefId, int? branchId)
        {

            ContextMenuService.Close();

            isProgess = true;

            StateHasChanged();
        }


        private List<LoanCurrencySummary> _summaries = [];

        void GetLoanCount(int count)
        {
            loanCount = count;
        }

        void GetSummaryChanged(List<LoanCurrencySummary> summaries)
        {
            _summaries = summaries;
            StateHasChanged();
        }

        void GetLoanDetails(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loans/Details/{p.LoanId}?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
        }
        void GetPaymentSchedules(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loans/{p.LoanId}/Repayment?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
            return;
        }

        void ChangeCreditAgent(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
            return;
        }
        void CustomerPayOff(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
            return;
        }


    }
}
