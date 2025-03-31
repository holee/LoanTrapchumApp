using Applications.Interfaces.Loans;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class ClientDuePayTab
    {
        readonly ILoanScheduleServiceImpl _scheduleService;
        private ILoanService _loanService;

        public ClientDuePayTab(ILoanScheduleServiceImpl loanScheduleService, ILoanService loanService)
        {
            this._scheduleService = loanScheduleService;
            _loanService = loanService;
        }

        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public long UserId { get; set; } = 0;
        [Parameter] public EventCallback<int> GetCountChanged { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        private List<VSchedule> ScheduleItems = [];
        private IList<VLoanDisbursments> SelectedLoans = [];
        protected IEnumerable<VBranchResponse> Branches = [];
        protected IEnumerable<VSchedulePaid> PaidSchedules = [];
        protected string filterText = string.Empty;
        int counts = 0;
        int pageSize = 10;
        bool isLoading = false;
        bool isProgess = false;
        bool includeInactive = false;
        string sortField = "LoanId";
        bool sortDescending = true;
        int pageIndex = 1;
        DateTime paymentDate = DateTime.Now;
        void ChangedPageSize(int page)
        {
            pageSize = page;
        }
        void RowSelected(VSchedule schedule) { }
        async Task GetCount()
        {
            await GetCountChanged.InvokeAsync(counts);
        }
        async void TextLoanSearchChanged(string filterText)
        {
            filterText = filterText ?? string.Empty;
            await GetLoans();
        }

        async void CheckedIncludeChanged()
        {
            await GetLoans();
        }

        void GetPaymentSchedules(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loans/Repayment/{p.LoanId}?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }

        void CustomerPayOff(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }
        protected override async Task OnInitializedAsync()
        {
            await GetLoans();
            await GetCount();
            await base.OnInitializedAsync();
        }
        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip!.Value;

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];

                string dir = args.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }
            await GetLoans();
        }

        private async Task GetLoans()
        {
            try
            {
                isLoading = true;

                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, filterText);

                var res = await _scheduleService.GetLoanSchedulesByPaymentDateAsync(paymentDate, BranchId, false, false, param);

                ScheduleItems = res.Items;

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


       
    }
}

//async Task DownLoadScheduleAsExcel(long loanRefId, int branchId)
//{
//    ContextMenuService.Close();
//    isProgess = true;

//    var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, branchId);
//    using var streamRef = new DotNetStreamReference(stream: stream);
//    await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);
//    isProgess = false;
//    StateHasChanged();
//}

//async Task DownLoadScheduleAsPdf(long loanRefId, int branchId)
//{

//    ContextMenuService.Close();

//    isProgess = true;
//    var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, branchId);

//    using var streamRef = new DotNetStreamReference(stream: stream);

//    await _js.InvokeVoidAsync("downloadFileFromStream", "Schedule.pdf", streamRef);
//    isProgess = false;
//    StateHasChanged();
//}