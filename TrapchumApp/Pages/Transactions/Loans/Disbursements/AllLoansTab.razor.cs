using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Loans.Responses;
using Applications.Features.Loans.Services;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Microsoft.JSInterop;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
{
    public partial class AllLoansTab
    {
        private readonly ILoanServiceImpl _dService;

        public AllLoansTab(ILoanServiceImpl dService)
        {
            _dService = dService;
        }

        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }
        [Parameter] public EventCallback<int> GetLoanChanged { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;

        private List<VLoanDisbursments> LoanItems = [];

        private IList<VLoanDisbursments> SelectedLoans = [];

        private string? filter = string.Empty;

        protected IEnumerable<VBranchResponse> Branches = [];

        private int selectedBranchId;

        bool branchStatus = false;

        int counts = 0;

        int pageSize = 15;

        bool isLoading = false;

        bool isProgess = false;

        bool includeInactive = false;

        string sortField = "RegisterDate";

        bool sortDescending = true;

        int pageIndex = 0;

        bool _isCreditAgent = false;
        async Task ChangedPageSize(int page)
        {
            pageSize = page;
            pageIndex = 0;
            await GetLoanDisbursements();
        }
        async void TextLoanSearchChanged(string filterText)
        {
            filter = filterText;
            await GetLoanDisbursements();
        }
        async void CheckedIncludeChanged()
        {
            await GetLoanDisbursements();
        }

        protected override async Task OnInitializedAsync()
        {

            await GetLoanDisbursements();

            await GetLoanCount();

            await base.OnInitializedAsync();
        }

        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;

            (sortField, sortDescending) = args.PageInfo();

            await GetLoanDisbursements();
        }

        private async Task GetLoanDisbursements()
        {
            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filter);

                isLoading = true;

                var auth = await AuthenticationState;

                await bState.CheckedIsCreditAgent(auth.User);

                var userId = bState.IsCreditAgent ? UserId : string.Empty;

                var res = await _dService.GetLoansByBranchIdAsync(BranchId, userId, LoanStatuses.Active, param);

                LoanItems = res.Items;

                counts = res.Count;

                isLoading = false;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            finally
            {
                isLoading = false;
            }

        }

        async Task GetLoanCount()
        {
            await GetLoanChanged.InvokeAsync(counts);
        }

    }
}



//async Task DownLoadScheduleAsExcel(string? loanRefId, int? branchId)
//{
//    ContextMenuService.Close();

//    if (loanRefId.IsNotNullOrEmpty() && branchId > 0)
//    {
//        await js.InvokeVoidAsync("open", $"/Reports/Schedules/{loanRefId}/{branchId}/Exel", "_blank");
//    }
//}

//async Task DownLoadScheduleAsPdf(string? loanRefId, int? branchId)
//{

//    ContextMenuService.Close();

//    if (loanRefId.IsNotNullOrEmpty() && BranchId > 0)
//    {
//        await js.InvokeVoidAsync("open", $"/Reports/Schedules/{loanRefId}/{branchId}", "_blank");
//    }
//}
//async void ShowPaidSchedules(long loanId, int branchId)
//{
//    ContextMenuService.Close();

//    var response = await _dService.GetPaymentSchedulesByLoanId(loanId, branchId);

//    if (response.IsSuccess)
//    {
//        PaidSchedules = response.Data!;
//    }
//    else
//    {

//        PaidSchedules = [];

//        var error = response.Errors!.FirstOrDefault();

//        NotificationService.Notify(NotificationSeverity.Error, error);
//    }
//    StateHasChanged();
//}


//void GetLoanDetails(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/App/Loan/{p.LoanId}/Branches/{BranchId}/Currencies/{p.CurrencyId}");
//}
//void GetPaymentSchedules(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/App/Loans/{p.BranchId}/Branches/{p.LoanId}/Schedules?CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//}

//void ChangeCreditAgent(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/App/Loan/{p.LoanId}/CreditAgent/{p.CreditAgentRefId}?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//}
//void CustomerPayOff(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/{p.CreditAgentRefId}?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//    return;
//}