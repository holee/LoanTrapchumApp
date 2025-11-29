using Applications.Enumerations;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.OpenBranches;
using Applications.Services.Transactions.Branches;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Branches
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
        private string? _userId { get; set; }
        IEnumerable<VCurrencyCounter> CurrencyNotes = [];
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

            _userId = auth.GetUserId();

            canOpenBranch = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.OpenBranch);

            canCloseBranch = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.CloseBranch);

            canVaultOut = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.TranxVaultOut);

            canVaultIn = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.TranxVaultIn);

            var rsp = await obService.GetCashVaultBalance(BranchId);

            if (rsp.IsSuccess)
            {
                CashVaultBalances = rsp.Data ?? [];
            }

            if (CashVaultBalances.Any())
            {
                await GetCurrencies((int)CurrenciesType.Khr);

                await GetCurrencyNotes(currencyId, BranchId);
            }



            await bState.GetBranchState(BranchId);

            await base.OnInitializedAsync();
        }

        async Task GetCurrencies(int id = 0)
        {
            var rsp = await curService.GetCurrencies<VCurrencyList>(id);

            if (rsp.IsSuccess)
            {

                currencyId = rsp.Data!.First().CurrencyId;

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
                UserRefId = _userId,
                BranchRefId = BranchId,
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
            await GetCurrencyNotes(currencyId!.Value, BranchId);
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
            var openBranch = new OpenBranchTransactionDto()
            {
                TransactionsId = $"trx-{Guid.CreateVersion7()}",
                TranTypeRefId = 68,
                TransactionTypeName = "Open Branch",
                UserRefId = _userId,
                BranchRefId = BranchId,
                OpenBranch = new OpenBranchDtoCreate()
                {
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
                    { "UserId", _userId },
                    { "BranchId", BranchId },
                                                                                                                                                                                                                },
            new DialogOptions
            {
                ShowTitle = false,
                Width = "100vw",
                Height = "100vh",
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