using Applications.Extensions;
using Applications.Features.Loans.Services;
using Applications.Requests.Loans.CreditAgents;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Services.Lists;
using Domains.Models.Identity;
using System.Threading.Tasks;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
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

        [Parameter] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        IEnumerable<VCreditAgent> _oldCreditAgents = [];

        IEnumerable<VCreditAgent> _newCreditAgents = [];

        IEnumerable<VLoanDisbursments> _items = [];

        IList<VLoanDisbursments>? _selectedSchedules;

        IEnumerable<VBranchResponse>? _newBranches = [];

        string? _newCreditAgentId = null;

        string? UserId = null;

        int _pageSize = 15;

        VLoanDisbursments _existingLoan = new();

        int _counts = 0;

        bool _isLoading = false;

        bool disabledOldCreditAgent = false;//=> !string.IsNullOrWhiteSpace(CreditAgentId);
        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthenticationState;

            UserId = auth.User.GetId();

            //await GetOldCreditAgents();

            if (!string.IsNullOrWhiteSpace(CreditAgentId))
            {
                await GetLoanByCreditAgents(CreditAgentId);

                disabledOldCreditAgent = true;
            }


            await GetBranches();


            await base.OnInitializedAsync();

        }


        async Task OnSelectedItemChanged(object? arg)
        {
            if (arg is not null)
            {

                BranchId = ((VBranchResponse)arg).BranchId;

                await GetOldCreditAgents();

            }
            _newCreditAgents = [];
            _items = [];
        }



        async Task GetBranches(int? branchId = 0)
        {
            var rsp = await _branchService.GetBranchesAsync<VBranchResponse>(branchId, new QueryParam(0, 0));

            if (rsp.IsSuccess)
            {
                _newBranches = rsp.Data.Items ?? [];
            }
        }

        async Task GetLoansByCreditAgents(object? arg)
        {

            if (arg is null)
            {
                _isLoading = false;

                return;
            }

            var creditAgentId = ((VCreditAgent)arg).EmployeeRefId;

            await GetLoanByCreditAgents(creditAgentId);

        }



        async Task GetLoanByCreditAgents(string? creditAgentId = null)
        {

            _isLoading = true;

            var resp = await _loanService.GetLoanByCreditAgentsAsync(creditAgentId, BranchId);

            _items = resp.Items;

            _counts = resp.Count;

            await GetNewCreditAgents();

            _isLoading = false;
        }


        async Task GetOldCreditAgents(string? filterText = null)
        {

            var param = new QueryParam(0, 0, nameof(VCreditAgent.AltName), false, false, filterText);

            var rsp = await _creditAgentService.GetCreditAgents(BranchId, param);

            _oldCreditAgents = rsp.Items ?? [];

        }
        async Task GetNewCreditAgents(string? filterText = null)
        {

            var param = new QueryParam(0, 0, nameof(VCreditAgent.AltName), false, false, filterText);

            var rsp = await _creditAgentService.GetCreditAgents(BranchId, param);

            if (rsp.Items.Any() && CreditAgentId.IsNotNullOrEmpty())
            {
                _newCreditAgents = rsp.Items.Where(x => x.EmployeeRefId != CreditAgentId);
            }
        }

        async Task ChangedCreditAgent()
        {
            if (_newCreditAgentId.IsNullOrEmpty() || CreditAgentId.IsNullOrEmpty() || !_selectedSchedules!.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "operation is failed.");
                return;
            }

            var models = _selectedSchedules?
                                .Where(x => x.LoanId != null)
                                .Select(x => new LoanChangedCaDtoCreate(x.LoanId, CreditAgentId, _newCreditAgentId, UserId, UserId))
                                .ToList();

            if (models is null || !models.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "operation is failed.");

                return;
            }

            var resp = await _creditAgentService.ChangeCreditAgents(BranchId, models);

            if (resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, resp.Data);

                await GetLoanByCreditAgents(CreditAgentId);

                return;
            }

            _isLoading = false;

            NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);

        }
        void Back()
        {
            NavigationManager.NavigateTo($"/Loans?BranchId={BranchId}&UserId={UserId}");
        }

    }
}