using Applications.Enumerations;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.OpenBranches;
using Applications.Responses.ViewModels.Currencies;
using Applications.Services.Currencies;
using Applications.Services.Transactions.Branches;
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
        readonly IOpenBranchServiceImpl obService;
        readonly ICurrencyServiceImpl curService;
        public OpenBranch(IOpenBranchServiceImpl obService, ICurrencyServiceImpl curService)
        {
            this.obService = obService;
            this.curService = curService;
        }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
        IEnumerable<VCurrencyNotes> CurrencyNotes = [];
        IEnumerable<VCashVaultBalance> CashVaultBalances = [];
        OpenBranchTransactionDto? transactionOpenBranch;
        TransactionCloseBranchDto? transactionCloseBranch;
        bool isClicked = false;
        long userRefId = 0;
        int currencyId = 1;
        bool canOpenBranch = false;
        bool canCloseBranch = false;
        bool canVaultOut = false;
        bool canVaultIn = false;
        private AlertStyle alertStyle;

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;
           
            canOpenBranch = await _authorizeService.MustHasPermissionAsync(auth.User, TranxsPolicies.OpenBranch);
           
            canCloseBranch = await _authorizeService.MustHasPermissionAsync(auth.User, TranxsPolicies.CloseBranch);
           
            canVaultOut = await _authorizeService.MustHasPermissionAsync(auth.User, TranxsPolicies.VaultOut);
           
            canVaultIn = await _authorizeService.MustHasPermissionAsync(auth.User, TranxsPolicies.VaultIn);

            var balanceResp = await obService.GetCashVaultBalance(BranchId);

            if (balanceResp.IsSuccess)
            {
                CashVaultBalances = balanceResp.Data ?? [];
            }

            if (CashVaultBalances.Any())
            {
                await  GetCurrencies((int)CurrenciesType.Khr);
                await GetCurrencyNotes(currencyId, BranchId);
            }

           

            await bState.GetBranchState(BranchId);

            await base.OnInitializedAsync();
        }

        async Task GetCurrencies(int id=0)
        {
            var rsp = await curService.GetCurrencies<VCurrencyList>(id);

            if (rsp.IsSuccess) { 

                currencyId=rsp.Data!.First().CurrencyId;

            } 

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
                TransactionsId = $"trx_{Guid.CreateVersion7()}",
                TransactionTypeName = "Close Branch",
                TranTypeRefId = 68,
                UserRefId = UserId,
                OpenBranch = new CloseBranchDtoUpdate
                {
                    OpenBranchId = $"trx-{Guid.CreateVersion7()}",
                    BranchRefId = BranchId,
                    OpenDate = DateTime.Now,
                    BankRefId = 11,
                    OpenStatus = "0",
                }
            };

            var result = await obService.CloseBranchAsync(transactionCloseBranch);

            if (result!.IsSuccess)
            {

                bState.State = false;

                NotificationService.Notify(NotificationSeverity.Success, result.Data);

            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }
        }

        async Task OpenSideDialog(int? currencyId)
        {
           await GetCurrencyNotes(currencyId!.Value,BranchId);
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
           var  openBranch = new OpenBranchTransactionDto()
            {
                TransactionsId = $"trx-{Guid.CreateVersion7()}",
                TranTypeRefId = 68,
                TransactionTypeName = "Open Branch",
                UserRefId = UserId,
                OpenBranch = new OpenBranchDtoCreate() {
                    OpenBranchId = $"trx-{Guid.CreateVersion7()}",
                    BankRefId = 11,
                    BranchRefId = BranchId,
                    OpenStatus = "1",
                    OpenDate = DateTime.Now
                }
            };

            var result = await obService.OpenBranchAsync(openBranch);

            if (result!.IsSuccess)
            {
                bState.State = true;
                NotificationService.Notify(NotificationSeverity.Success, result.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }

        }

        async Task GetCurrencyNotes(int currencyId, int? branchRefId)
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
                new Dictionary<string, object?> {
                    { "UserId", UserId },
                    { "BranchId", BranchId },
                                                                                                                                                                                                                },
            new DialogOptions
            {
                ShowTitle = false,
                Style = "min-width:75rem"
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