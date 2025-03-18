using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Currency;
using Applications.Requests.Lists.Entities;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Currencies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Branches
{
    public partial class CashVaultModalActions
    {
        [Inject] ICurrencyService curService { get; set; } = default!;
        [Inject] ICashVaultService caVaultService { get; set; } = default!;

        [CascadingParameter]
        Task<AuthenticationState> AuthState { get; set; } = default!;
        [Parameter, SupplyParameterFromQuery] public string UserId { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }

        protected IEnumerable<BankDto>? bankList = [];

        private IEnumerable<VCashVaults>? vCashVaults = [];

        protected IEnumerable<VBranch> Branches = [];

        protected BankDto? bank;

        protected BranchDto? branch;

        protected int? CurrencyId;

        private bool IsDisabled = false;
        bool canVaultOut = false;
        bool canVaultIn = false;

        DialogOptions options = new DialogOptions
        {
            ShowTitle = false,
            Style="min-width:75rem"
        };
        protected override async Task OnInitializedAsync()
        {
            try
            {

                var user = (await AuthState).User;

                //canVaultOut = await _authorizeService.MustHasPermissionAsync(user, TranxPolicies.VaultOut);

                //canVaultIn = await _authorizeService.MustHasPermissionAsync(user, TranxPolicies.VaultIn);

                var branchResponse = await _branchService.GetBranchesByUser(UserId);

                if (branchResponse.IsSuccess)
                {
                    Branches = branchResponse.Data!;
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
            var resultModal = await DialogService.OpenAsync<VaultOutModal>("Vault Out",
                new Dictionary<string, object> {
                    { "UserId", UserId },
                    { "BranchId", BranchId },
                }, options);
            if (resultModal != null)
            {
                await OnInitializedAsync();
                Console.WriteLine("Vault Out...");
            }

        }
        protected async void OnCashVaultIn()
        {
            var resultModal = await DialogService.OpenAsync<VaultInModal>("Vault In",
                new Dictionary<string, object> {
                    { "UserId", UserId },
                    { "BranchId", BranchId },
                }, options);
            if (resultModal != null)
            {
                await OnInitializedAsync();
                Console.WriteLine("Vault In...");
            }
        }
    }
}