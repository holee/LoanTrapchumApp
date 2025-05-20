using Applications.Interfaces.Loans;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class ClientAllPayTab
    {
        private readonly ILoanServiceImpl _dService;
        private readonly ILoanScheduleServiceImpl _loanScheduleService;
        public ClientAllPayTab(ILoanServiceImpl loanService, 
                              ILoanScheduleServiceImpl loanScheduleService)
        {
            _dService = loanService;
            _loanScheduleService = loanScheduleService;
        }

        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; } = string.Empty;
        [Parameter] public EventCallback<string> ShowCounts { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        private List<VSchedule> ScheduleItems = [];
        private IList<VLoanDisbursments> SelectedLoans = [];
        private string? filter = string.Empty;
        protected IEnumerable<VBranchResponse> Branches = [];
        protected string filterText = string.Empty;
        int counts = 0;
        int pageSize = 10;
        int pageIndex = 1;
        bool isLoading = false;
        bool isProgess = false;
        bool includeInactive = false;
        string sortField = "LoanId";
        bool sortDescending = true;

        DateTime paymentDate = DateTime.Now;
        void ChangedPageSize(int page)
        {
            pageSize = page;
        }

        async Task GetCount()
        {
            await ShowCounts.InvokeAsync(counts.ToString());
        }
        async void TextLoanSearchChanged(string filter)
        {
            this.filter = filter;
            await GetLoansAsync();
        }

        async void CheckedIncludeChanged()
        {
            await GetLoansAsync();
        }
        void GetPaymentSchedules(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loans/{p.LoanId}/Schedules?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }

        void CustomerPayOff(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }
        protected override async Task OnInitializedAsync()
        {
            await GetLoansAsync();
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
            await GetLoansAsync();
        }

        private async Task GetLoansAsync()
        {
            try
            {
                isLoading = true;

                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, filter);

                var res = await _loanScheduleService.GetLoanSchedulesByPaymentDateAsync( BranchId, paymentDate, true, false, param);

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

        void RowSelected(VSchedule schedule) { }

    }
}


//async Task DownLoadScheduleAsExcel(Guid loanRefId, int branchId)
//{
//    ContextMenuService.Close();
//    isProgess = true;

//    var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, branchId);
//    using var streamRef = new DotNetStreamReference(stream: stream);
//    await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);
//    isProgess = false;
//    StateHasChanged();
//}



//async Task DownLoadScheduleAsPdf(Guid loanRefId, int branchId)
//{

//    ContextMenuService.Close();

//    isProgess = true;
//    var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, branchId);

//    using var streamRef = new DotNetStreamReference(stream: stream);

//    await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.pdf", streamRef);
//    isProgess = false;
//    StateHasChanged();
//}