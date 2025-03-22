using Applications.Requests.Loans.CreditAgents;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Services.Lists;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanChangedCreditAgent
    {

        private readonly ICreditAgentServiceImpl _creditAgentService;
        private readonly ILoanServiceImpl _loanDisbursementService;
        public LoanChangedCreditAgent(ICreditAgentServiceImpl creditAgentService, ILoanServiceImpl loanDisbursementService)
        {
            _creditAgentService = creditAgentService;
            _loanDisbursementService = loanDisbursementService;
        }

        [Parameter]
        public long LoanId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public long CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public long CurrencyId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public long CreditAgentId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public long UserId { get; set; }

        IEnumerable<VCreditAgent> CreditAgents = [];
        long newCreditAgentId = 0;
        VLoanDisbursments existingLoan = new();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // var response = await _loanDisbursementService.GetLoanByIdAsync(LoanId, BranchId);

            // if (response.IsSuccess)
            // {
            //     existingLoan = response.Data!;
            // }
            await GetCreditAgents();

        }

        async Task GetCreditAgents(LoadDataArgs e)
        {

            var filterText = e.Filter.Trim() ?? "";

            await GetCreditAgents(filterText);
        }

        async Task GetCreditAgents(string? filterText = null)
        {
            var creditAgentResult = await _creditAgentService.GetCreditAgents(false, filterText);

            if (creditAgentResult.Any())
            {
                CreditAgents = creditAgentResult.Where(x => x.EmployeeRefId != CreditAgentId);
            }
        }

        async Task ChangedCreditAgent()
        {
            if (newCreditAgentId <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please select new credit agent.");
                return;
            }

            var model = new List<LoanChangedCaDtoCreate>{

                     new(LoanId, CreditAgentId, newCreditAgentId, UserId, UserId)
             };

            var resp = await _loanDisbursementService.ChangedCreditAgent(BranchId, model);

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