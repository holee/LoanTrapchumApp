using Applications.Extensions;
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
        public string? LoanId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public long CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public long CurrencyId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public string? CreditAgentId { get; set; }
        [Parameter, SupplyParameterFromQuery]
        public string? UserId { get; set; }

        private IEnumerable<VCreditAgent> _creditAgents = [];
        private string? newCreditAgentId = string.Empty;
        private VLoanDisbursments existingLoan = new();
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
            var rsp = await _creditAgentService.GetCreditAgents(false, filterText);

            if (rsp.IsSuccess)
            {
                _creditAgents = rsp.Data!.Where(x => x.EmployeeRefId != CreditAgentId);
            }
        }

        async Task ChangedCreditAgent()
        {
            if (newCreditAgentId.IsNullOrEmpty())
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