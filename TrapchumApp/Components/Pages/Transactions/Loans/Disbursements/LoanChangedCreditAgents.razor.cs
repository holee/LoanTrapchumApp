using Applications.Enumerations;
using Applications.Extensions;
using Applications.Requests.Loans.CreditAgents;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Services.Lists;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanChangedCreditAgents
    {

        private readonly ILoanServiceImpl _loanService;
        private readonly ICreditAgentServiceImpl _creditAgentService;
        public LoanChangedCreditAgents(ILoanServiceImpl loanService, ICreditAgentServiceImpl creditAgentService)
        {
            _loanService = loanService;
            _creditAgentService = creditAgentService;
        }

        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }

        IEnumerable<VCreditAgent> _oldCreditAgents = [];
        IEnumerable<VCreditAgent> _newCreditAgents = [];
        IEnumerable<VLoanDisbursments> _items = [];
        string? _newCreditAgentId = null;
        string? _oldCreditAgentId = null;
        int _pageIndex = 0;
        int _pageSize = 10;
        string _sortColumn = "RegisterDate";
        bool _sortDescending = true;
        VLoanDisbursments _existingLoan = new();
        int _counts = 0;
        bool _isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetOldCreditAgents();

        }

        async Task GetLoansByCreditAgents()
        {

            _isLoading = true;

            var param = new QueryParam(_pageIndex, _pageSize, _sortColumn, true, false, string.Empty);

            var resp = await _loanService.GetLoansAsync(BranchId, _oldCreditAgentId, LoanStatuses.Active, param);


            _items = resp.Items;

            _counts = resp.Count;

            await GetNewCreditAgents();

            _isLoading = false;
        }




        async Task GetOldCreditAgents(LoadDataArgs e)
        {

            var filterText = e.Filter.Trim() ?? "";

            await GetOldCreditAgents(filterText);
        }

        async Task GetNewCreditAgents(LoadDataArgs e)
        {

            var filterText = e.Filter.Trim() ?? "";

            await GetNewCreditAgents(filterText);
        }


        async Task GetOldCreditAgents(string? filterText = null)
        {
            var rsp = await _creditAgentService.GetCreditAgents(false, filterText);

            if (rsp.IsSuccess)
            {
                _oldCreditAgents = rsp.Data!;
            }
        }
        async Task GetNewCreditAgents(string? filterText = null)
        {
            var rsp = await _creditAgentService.GetCreditAgents(false, filterText);

            if (rsp.IsSuccess && _oldCreditAgentId.IsNotNullOrEmpty())
            {
                _newCreditAgents = rsp.Data!.Where(x => x.EmployeeRefId != _oldCreditAgentId);
            }
        }

        async Task ChangedCreditAgent()
        {
            if (_newCreditAgentId.IsNullOrEmpty() || _oldCreditAgentId.IsNullOrEmpty() || !_items.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "operation is failed.");
                return;
            }
            var models = _items.Select(x => new LoanChangedCaDtoCreate(x.LoanId, _oldCreditAgentId, _newCreditAgentId, UserId, UserId))
                                .ToList();
            var resp = await _loanService.ChangedCreditAgent(BranchId, models);
            if (resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, resp.Data);

                NavigationManager.NavigateTo($"/Loans?BranchId={BranchId}&UserId={UserId}");

                return;
            }

            NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);

        }
        void Back()
        {
            NavigationManager.NavigateTo($"/Loans?BranchId={BranchId}&UserId={UserId}");
        }

    }
}