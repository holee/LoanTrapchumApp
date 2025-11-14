using Applications.Extensions;
using Applications.Features.Loans.Services;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Lists;
using Domains.Models.Identity;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Loans.Responses;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
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
        [Parameter] public string? CreditAgentId { get; set; }
        [Parameter] public string? LoanId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        private string? UserId = string.Empty;
        private IEnumerable<VCreditAgent> _creditAgents = [];
        private string? newCreditAgentId = string.Empty;
        private VLoanDisbursments existingLoan = new();
        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthenticationState;

            UserId = auth.User.GetId();

            var response = await _loanDisbursementService.GetLoanByCreditAgentIdAsync(CreditAgentId, LoanId, BranchId);

            if (response.IsSuccess)
            {
                existingLoan = response.Data!;
            }

            await GetCreditAgents();


            await base.OnInitializedAsync();
        }



        async Task OnLoadData(LoadDataArgs arg)
        {
            await GetCreditAgents(arg.Filter ?? string.Empty);

            Console.WriteLine(arg.Filter);
        }


        async Task GetCreditAgents(string? filterText = null)
        {

            var param = new QueryParam(0, 0, nameof(VCreditAgent.AltName), false, false, filterText);

            var rsp = await _creditAgentService.GetCreditAgents(BranchId, param);

            _creditAgents = rsp.Items.Where(x => x.EmployeeRefId != CreditAgentId);

        }

        async Task ChangedCreditAgent()
        {
            if (newCreditAgentId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please select new credit agent.");

                return;
            }

            var model = new List<LoanChangedCaDtoCreate>
            {
                     new(LoanId, CreditAgentId, newCreditAgentId, UserId, UserId)
             };

            var resp = await _creditAgentService.ChangeCreditAgents(BranchId, model);

            if (resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, resp.Data);

                return;
            }

            NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);

        }
        void Back()
        {
            NavigationManager.NavigateTo($"/App/Loans?BranchId={BranchId}&UserId={UserId}");
        }

    }
}