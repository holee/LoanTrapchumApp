using Applications.Features.Branches.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;
using Applications.Features.Loans.Services;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Domains.Models.Identity;
using TrapchumApp.Helpers.Enums;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
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
        [Parameter] public EventCallback<string> ShowCounts { get; set; }
        [Parameter] public EventCallback<VSchedule> ShowLateScheduleDetails { get; set; }
        [Parameter] public EventCallback<VSchedule> RecievePayment { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        private List<VSchedule> ScheduleItems = [];
        private IList<VLoanDisbursments> SelectedLoans = [];
        private string? filter = string.Empty;
        protected IEnumerable<VBranch> Branches = [];
        protected string filterText = string.Empty;
        private string? _userId { get; set; } = string.Empty;
        int counts = 0;
        int pageSize = 15;
        int pageIndex = 1;
        bool isLoading = false;
        bool isProgess = false;
        bool includeInactive = false;
        string sortField = "LoanId";
        bool sortDescending = true;

        DateTime paymentDate = DateTime.UtcNow;
        private DateTime? _start;
        private DateTime? _end;
        private int _selectedFilter = 0;

        void OnStartDateChanged()
        {
            var daysToAdd = _selectedFilter switch
            {
                (int)ScheduleFilter.MonthToDate => 28,

                (int)ScheduleFilter.NextWeek => 7,

                _ => (int?)null
            };

            if (daysToAdd.HasValue)
            {
                _end = _start!.Value.AddDays(daysToAdd.Value);
            }

            _ = OnFilterChanged();
        }

        async Task OnFilterChanged()
        {

            if (_selectedFilter == (int)ScheduleFilter.All)
            {
                await GetLoansAsync();
            }

            if (_selectedFilter == (int)ScheduleFilter.Today)
            {
                await GetLoanSchedulesDueDate();
            }

            if (_selectedFilter == (int)ScheduleFilter.OverDueDate)
            {
                await GetLoanSchedulesOverDueDate();
            }

            if (_selectedFilter == (int)ScheduleFilter.MonthToDate)
            {
                await GetLoanSchedulesMonthToDate();
            }

            if (_selectedFilter == (int)ScheduleFilter.NextWeek)
            {
                await GetLoanSchedulesNextWeek();
            }
            await GetCount();

        }

        async Task ChangedPageSize(int page)
        {
            pageSize = page;

            pageIndex = 0;

            await GetLoansAsync();
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

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            _userId = auth.User.GetId();

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
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filter);

            var query = new GetLoanSchedulesByPaymentDateQuery(BranchId, paymentDate, true, false, param);

            var res = await mediator.Send(query);

            ScheduleItems = res.Items;

            counts = res.Count;

            isLoading = false;

            StateHasChanged();
        }


        private async Task GetLoanSchedulesDueDate()
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filterText);

            var query = new GetLoanSchedulesByPaymentDateQuery(BranchId, DateTimeOffset.Now, false, false, param);

            var res = await mediator.Send(query);

            ScheduleItems = res.Items;

            counts = res.Count;

            isLoading = false;

            StateHasChanged();
        }


        private async Task GetLoanSchedulesOverDueDate()
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filterText);

            var query = new GetLoanSchedulesByPaymentDateQuery(BranchId, paymentDate, true, true, param);

            var res = await mediator.Send(query);

            ScheduleItems = res.Items;

            counts = res.Count;

            isLoading = false;

            StateHasChanged();
        }

        private async Task GetLoanSchedulesNextWeek()
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filterText);

            var query = new GetLoanSchedulesByPaymentDateQuery(BranchId, paymentDate, true, true, param);

            var res = await mediator.Send(query);

            ScheduleItems = res.Items;

            counts = res.Count;

            isLoading = false;

            StateHasChanged();
        }


        private async Task GetLoanSchedulesMonthToDate()
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filterText);

            var query = new GetLoanSchedulesMonthToDateQuery(BranchId, _start, _end, param);

            var res = await mediator.Send(query);

            ScheduleItems = res.Items;

            counts = res.Count;

            isLoading = false;

            StateHasChanged();
        }


        void RowSelected(VSchedule schedule) { }

    }
}


//void GetPaymentSchedules(VSchedule p)
//{
//    NavigationManager.NavigateTo($"/App/Loans/{p.BranchId}/Branches/{p.LoanId}/Schedules?CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
//    return;
//}
//void CustomerPayOff(VSchedule p)
//{
//    NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
//    return;
//}