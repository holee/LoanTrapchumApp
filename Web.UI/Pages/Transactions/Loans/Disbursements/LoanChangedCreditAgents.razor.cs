using Applications.Extensions;
using Applications.Features.Loans.Services;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Lists;
using Domains.Models.Identity;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Loans.Dtos.Responses;

namespace Web.UI.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanChangedCreditAgents(ILoanServiceImpl loanService, ICreditAgentServiceImpl creditAgentService)
    {
        [Parameter] public int? BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        IEnumerable<VCreditAgent> _oldCreditAgents = [];

        IEnumerable<VCreditAgent> _newCreditAgents = [];

        IEnumerable<VLoanDisbursments> _items = [];

        IList<VLoanDisbursments>? _selectedSchedules;

        IEnumerable<VBranch>? _newBranches = [];

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

            await GetOldCreditAgents();

            if (!string.IsNullOrWhiteSpace(CreditAgentId))
            {
                await GetLoanByCreditAgents(CreditAgentId);

                disabledOldCreditAgent = true;
            }


            await GetBranches(BranchId);


            await base.OnInitializedAsync();

        }


        async Task OnSelectedItemChanged(object? arg)
        {
            if (arg is not null)
            {

                BranchId = ((VBranch)arg).BranchId;

                await GetOldCreditAgents();

            }
            _newCreditAgents = [];

            _items = [];
        }



        async Task GetBranches(int? branchId = 0)
        {

            var command = new GetBranchAsPagingQuery(branchId, new QueryParam(0, 0));

            var rsp = await mediator.Send(command);

            _newBranches = rsp.Items ?? [];

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

            var resp = await loanService.GetLoanByCreditAgentsAsync(creditAgentId, BranchId);

            _items = resp.Items;

            _counts = resp.Count;

            await GetNewCreditAgents();

            _isLoading = false;
        }


        async Task GetOldCreditAgents(string? filterText = null)
        {

            var param = new QueryParam(0, 0, nameof(VCreditAgent.AltName), false, false, filterText);

            var rsp = await creditAgentService.GetCreditAgents(BranchId, param);

            _oldCreditAgents = rsp.Items ?? [];

        }
        async Task GetNewCreditAgents(string? filterText = null)
        {

            var param = new QueryParam(0, 0, nameof(VCreditAgent.AltName), false, false, filterText);

            var rsp = await creditAgentService.GetCreditAgents(BranchId, param);

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

            var resp = await creditAgentService.ChangeCreditAgents(BranchId, models);

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
            NavigationManager.NavigateTo(_navService.PreviousUrl!);
        }

    }
}