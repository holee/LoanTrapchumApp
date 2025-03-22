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
        [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }

        IEnumerable<VCreditAgent> OldCreditAgents = [];
        IEnumerable<VCreditAgent> NewCreditAgents = [];
        IEnumerable<VLoanDisbursments> Items = [];
        Guid? newCreditAgentId = null;
        Guid? oldCreditAgentId = null;
        int pageIndex = 0;
        int pageSize = 10;
        string sortColumn = "RegisterDate";
        bool sortDescending = true;
        VLoanDisbursments existingLoan = new();
        int counts = 0;
        bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetOldCreditAgents();

        }

        async Task GetLoansByCreditAgents()
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, true, false, string.Empty);

            var resp = await _loanService.GetLoansAsync(BranchId, oldCreditAgentId, LoanStatuses.Active, param);


            Items = resp.Items;

            counts = resp.Count;

            await GetNewCreditAgents();

            isLoading = false;
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
            var creditAgentResult = await _creditAgentService.GetCreditAgents(false, filterText);

            if (creditAgentResult.Any())
            {
                OldCreditAgents = creditAgentResult;
            }
        }
        async Task GetNewCreditAgents(string? filterText = null)
        {
            var creditAgentResult = await _creditAgentService.GetCreditAgents(false, filterText);

            if (creditAgentResult.Any() && oldCreditAgentId.NotEmpty())
            {
                NewCreditAgents = creditAgentResult.Where(x => x.EmployeeRefId != oldCreditAgentId);
            }
        }

        async Task ChangedCreditAgent()
        {
            if (newCreditAgentId.IsNullOrEmpty() || oldCreditAgentId.IsNullOrEmpty() || !Items.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "operation is failed.");
                return;
            }
            var models = Items.Select(x => new LoanChangedCaDtoCreate(x.Id, oldCreditAgentId, newCreditAgentId, UserId, UserId))
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