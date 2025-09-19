using Applications.Features.CashManagements.Responses;
using Applications.Features.Entities;
using Applications.Interfaces.Transactions;
using Applications.Responses.Lists.Currency;
using Applications.Responses.ViewModels.Branches;
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

        protected IEnumerable<VBranchResponse> Branches = [];

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

                var bResponse = await _branchService.GetBranchesByUser(_userId, 0);

                if (bResponse.IsSuccess)
                {
                    Branches = bResponse.Data!;
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