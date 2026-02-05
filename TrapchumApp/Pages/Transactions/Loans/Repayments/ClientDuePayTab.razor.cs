using Applications.Features.Branches.Dtos;
using Applications.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;
using Applications.Features.Loans.Responses;
using Applications.Features.Loans.Services;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
{
    public partial class ClientDuePayTab
    {
        private readonly ILoanScheduleServiceImpl _scheduleService;
        //private readonly ILoanServiceImpl _loanService;
        public ClientDuePayTab(ILoanScheduleServiceImpl loanScheduleService, ILoanServiceImpl loanService)
        {
            _scheduleService = loanScheduleService;
            //_loanService = loanService;
        }

        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public EventCallback<int> GetCounts { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        private List<VSchedule> ScheduleItems = [];
        private IList<VLoanDisbursments> SelectedLoans = [];
        protected IEnumerable<VBranch> Branches = [];
        protected string filterText = string.Empty;
        private string? _userId { get; set; } = string.Empty;
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
            await GetCounts.InvokeAsync(counts);
        }
        async void TextLoanSearchChanged(string filterText)
        {
            filterText = filterText ?? string.Empty;
            await GetLoanSchedulesDueDate();
        }

        async void CheckedIncludeChanged()
        {
            await GetLoanSchedulesDueDate();
        }

        void GetPaymentSchedules(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loans/Repayment/{p.LoanId}?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
            return;
        }

        void CustomerPayOff(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
            return;
        }
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            _userId = auth.User.GetId();

            await GetLoanSchedulesDueDate();

            await GetCount();

            await base.OnInitializedAsync();
        }
        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;

            (sortField, sortDescending) = args.PageInfo(nameof(VSchedule.PaymentDate));

            await GetLoanSchedulesDueDate();
        }

        private async Task GetLoanSchedulesDueDate()
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filterText);

            var query = new GetLoanSchedulesByPaymentDateQuery(BranchId, paymentDate, false, false, param);

            var res = await mediator.Send(query);

            ScheduleItems = res.Items;

            counts = res.Count;

            isLoading = false;

            StateHasChanged();
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