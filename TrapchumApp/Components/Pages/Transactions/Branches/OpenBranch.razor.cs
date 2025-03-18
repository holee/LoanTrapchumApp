using Applications.Enumerations;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.OpenBranches;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TrapchumApp.Components.Shared;

namespace TrapchumApp.Components.Pages.Transactions.Branches
{
    public partial class OpenBranch
    {
        [Inject] IOpenBranchService obService { get; set; } = default!;
        [Inject] ICurrencyService curService { get; set; } = default!;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public long UserId { get; set; }
        IEnumerable<VCurrencyNotes> CurrencyNotes = [];
        IEnumerable<VCashVaultBalance> CashVaultBalances = [];
        OpenBranchTransactionDto? transactionOpenBranch;
        TransactionCloseBranchDto? transactionCloseBranch;
        bool isClicked = false;
        long userRefId = 0;
        int selectedBranchRefId = 0;
        int selectedCurrencyRefId = 0;
        string branchName = "Siem Reap";
        int currencyId = 1;
        bool canOpenBranch = false;
        bool canCloseBranch = false;
        bool canVaultOut = false;
        bool canVaultIn = false;
        private AlertStyle alertStyle;
        bool isBranchOpen = false;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;
            canOpenBranch = await _authorizeService.MustHasPermissionAsync(auth.User, TranxPolicies.OpenBranch);
            canCloseBranch = await _authorizeService.MustHasPermissionAsync(auth.User, TranxPolicies.CloseBranch);
            canVaultOut = await _authorizeService.MustHasPermissionAsync(auth.User, TranxPolicies.VaultOut);
            canVaultIn = await _authorizeService.MustHasPermissionAsync(auth.User, TranxPolicies.VaultIn);

            var balanceResp = await obService.GetCashVaultBalance(BranchId);

            if (balanceResp.IsSuccess)
            {
                CashVaultBalances = balanceResp.Data ?? [];
            }


            currencyId = CashVaultBalances!.FirstOrDefault(x => x.CurrencyId == (int)CurrenciesType.Khr)!.CurrencyId!.Value;

            var currencyResponse = await curService.GetCurrencyNotes(currencyId, BranchId);

            if (currencyResponse.IsSuccess)
            {
                CurrencyNotes = currencyResponse.Data!;
            }
            else
            {
                CurrencyNotes = [];
            }

            await bState.GetBranchState(BranchId);

            await base.OnInitializedAsync();
        }

        async Task SubmitCloseBranch()
        {

            var confirm = await DialogService.Confirm("Are you sure you want to close branch?", "Confirm", new ConfirmOptions
            {
                OkButtonText = "Yes",
                CancelButtonText = "No",
                CloseDialogOnEsc = false,
            });
            if (confirm == false)
            {
                return;
            }

            transactionCloseBranch = new TransactionCloseBranchDto
            {
                TransactionTypeName = "Open Branch",
                TranTypeRefId = 68,
                UserRefId = UserId,
                OpenBranch = new CloseBranchDtoUpdate
                {
                    BranchRefId = BranchId,
                    OpenDate = DateTime.Now,
                    BankRefId = 11,
                    OpenStatus = "0"
                }
            };

            var tranx = transactionCloseBranch.Adapt<LoanTransaction>();

            var result = await obService.CloseBranchAsync(tranx);

            if (result!.IsSuccess)
            {

                bState.BranchState = false;

                NotificationService.Notify(NotificationSeverity.Success, result.ErrMessage);

            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }
        }

        async Task OpenSideDialog(int currencyId)
        {
            var currencyResponse = await curService.GetCurrencyNotes(currencyId, BranchId);

            if (currencyResponse.IsSuccess)
            {
                CurrencyNotes = currencyResponse.Data!;
            }
            else
            {
                CurrencyNotes = [];
            }
        }

        async Task SubmitOpenBranch()
        {
            var confirm = await DialogService.Confirm("Are you sure you want to open branch?", "Confirm", new ConfirmOptions
            {
                OkButtonText = "Yes",
                CancelButtonText = "No",
                CloseDialogOnEsc = false,
            });
            if (confirm == false)
            {
                return;
            }

            transactionOpenBranch = new OpenBranchTransactionDto
            {
                TranTypeRefId = 68,
                TransactionTypeName = "Open Branch",
                UserRefId = UserId,
                OpenBranch = new OpenBranchDtoCreate
                {
                    BankRefId = 11,
                    BranchRefId = BranchId,
                    OpenStatus = "1",
                    DelegateStaffId = UserId,
                    OpenDate = DateTime.Now
                }
            };

            var tranx = transactionOpenBranch.Adapt<LoanTransaction>();

            var result = await obService.OpenBranchAsync(tranx);

            if (result!.IsSuccess)
            {
                bState.BranchState = true;
                NotificationService.Notify(NotificationSeverity.Success, result.ErrMessage);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }

        }

        async Task GetCurrencyNotes(int currencyId, int branchRefId)
        {
            isClicked = !isClicked;

            this.currencyId = currencyId;

            var currencyResponse = await curService.GetCurrencyNotes(currencyId, branchRefId);

            if (currencyResponse.IsSuccess)
            {
                CurrencyNotes = currencyResponse.Data!;
            }
            else
            {
                CurrencyNotes = [];
            }
        }



        protected async void OnCashVaultOut()
        {
            var resultModal = await DialogService.OpenAsync<VaultOutModal>("Vault Out",
                new Dictionary<string, object> {
                    { "UserId", UserId },
                    { "BranchId", BranchId },
                                                                                                                                                                                                                },
            new DialogOptions
            {
                Draggable = true,
                Resizable = true,
                ShowTitle = false,
                ShowClose = false,
                Width = "70rem"
            });
            if (resultModal != null)
            {
                await OnInitializedAsync();
            }
            await InvokeAsync(StateHasChanged);

        }


        public void Dispose()
        {
           
        }

    }
}