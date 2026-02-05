using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchByUser;
using Applications.Features.CashManagements.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses.Lists.Currency;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class CashVaultModalActions
    {
        private readonly ICashVaultService caVaultService;
        public CashVaultModalActions(ICashVaultService caVaultService)
        {
            this.caVaultService = caVaultService;
        }

        [CascadingParameter]
        Task<AuthenticationState> AuthState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; }
        [Parameter] public int BankId { get; set; } = 11;

        private string? _userId = string.Empty;

        protected IEnumerable<BankResponse>? bankList = [];

        private IEnumerable<VCashVaults>? vCashVaults = [];

        protected IEnumerable<VBranch> Branches = [];

        protected BankResponse? bank;

        protected BranchDto? branch;

        protected int? CurrencyId;

        private bool IsDisabled = false;

        bool canVaultOut = false;

        bool canVaultIn = false;

        DialogOptions options = new DialogOptions
        {
            ShowTitle = false,
            Width = "100vw",
            Height = "100vh",
            AutoFocusFirstElement = false,
        };
        protected override async Task OnInitializedAsync()
        {
            try
            {

                var user = (await AuthState).User;

                _userId = (await AuthState).GetUserId();

                await bState.GetAuthenticatedUser(user);

                var command = new GetBranchByUserQuery(_userId, 0);

                var bResponse = await mediator.Send(command);

                if (bResponse.IsSuccess())
                {
                    Branches = bResponse.Value;
                }

                var resp = await caVaultService.GetCashVaultsAsync(BranchId);

                vCashVaults = resp.IsSuccess ? resp.Data : [];


            }
            catch (Exception ex)
            {
                NotificationService!.Notify(NotificationSeverity.Error, ex.Message);
            }
            await base.OnInitializedAsync();
        }

        protected async void OnCashVaultOut()
        {

            var param = new Dictionary<string, object?> {
                    { "BranchId", BranchId },
                    { "BankId", BankId },
                };

            var resultModal = await DialogService.OpenAsync<VaultOutModal>("Vault Out", param, options);

        }
        protected async void OnCashVaultIn()
        {

            var param = new Dictionary<string, object?> {
                    { "BranchId", BranchId },
                    { "BankId", BankId },
                };

            var resultModal = await DialogService.OpenAsync<VaultInModal>("Vault In", param, options);

            if (resultModal != null)
            {

            }
        }
    }
}