using Applications.Enumerations;
using Applications.Features.Branches.Command.CloseBranch;
using Applications.Features.Branches.Command.OpenBranch;
using Applications.Features.Branches.Queries.GetBranchState;
using Applications.Features.Branches.Queries.GetCashVaultBalance;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Queries.GetCurrencyCounter;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetCurrencyNotes;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.OpenBranches;
using Applications.Responses.Paginations;
using Applications.Services.Transactions.Branches;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class OpenBranch
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;
        [Parameter] public int? BranchId { get; set; } = 0;
        [PersistentState] public string? UserId { get; set; }
        [PersistentState] public IEnumerable<VCurrencyCounter>? CurrencyNotes { get; set; }
        [PersistentState] public IEnumerable<VCashVaultBalance>? CashVaultBalances { get; set; }
        OpenBranchTransactionDto? transactionOpenBranch;
        OpenBranchTransactionDto? transactionCloseBranch;
        bool isClicked = false;
        long userRefId = 0;
        int currencyId = 1;
        private AlertStyle alertStyle;
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                var auth = await AuthenticationState;

                UserId = auth.GetUserId();

                await bState.GetAuthenticatedUser(auth.User);
            }
            await GetCashVault();

            await GetBranchState();

            await base.OnInitializedAsync();
        }

        private async Task GetCashVault()
        {
            var command1 = new GetCashVaultBalanceQuery(BranchId);

            var rsp = await mediator.Send(command1);

            if (rsp.IsSuccess())
            {
                CashVaultBalances = rsp.Value ?? [];
            }

            if (CashVaultBalances is not null && CashVaultBalances.Any())
            {
                await GetCurrencies((int)CurrenciesType.Khr);

                await GetCurrencyNotes(currencyId, BranchId);
            }
        }

        private async Task GetBranchState()
        {
            var command = new GetBranchStateQuery(UserId);

            var rsp1 = await mediator.Send(command);

            if (rsp1.IsSuccess())
            {
                bState.State = rsp1.Value.IsOpen;
            }
            else
            {
                bState.State = false;
            }
        }

        async Task GetCurrencies(int id = 0)
        {

            var query = new GetCurrencyDetailsByIdQuery(id);

            var rsp = await mediator.Send(query);  //curService.GetCurrencies<VCurrencyList>(id);

            if (rsp.IsSuccess())
            {
                currencyId = rsp.Value.CurrencyId;
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

            transactionCloseBranch = new OpenBranchTransactionDto
            {
                TransactionsId = $"trx_{Guid.CreateVersion7()}",
                TransactionTypeName = "Close Branch",
                TranTypeRefId = 68,
                UserRefId = UserId,
                BranchRefId = BranchId,
                OpenBranch = new OpenBranchDtoCreate
                {
                    OpenBranchId = $"trx-{Guid.CreateVersion7()}",
                    BranchRefId = BranchId,
                    OpenDate = DateTime.Now,
                    BankRefId = 11,
                    OpenStatus = 0,
                }
            };

            var closeBranchCommand = new CloseBranchCommand(transactionCloseBranch);

            var result = await mediator.Send(closeBranchCommand); //obService.CloseBranchAsync(transactionCloseBranch);

            if (result.IsSuccess())
            {

                bState.State = false;

                NotificationService.Notify(NotificationSeverity.Success, "Close Branch successful");

            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.GetMessage());
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
                UserRefId = UserId,
                BranchRefId = BranchId,
                OpenBranch = new OpenBranchDtoCreate()
                {
                    OpenBranchId = $"trx-{Guid.CreateVersion7()}",
                    BankRefId = 11,
                    BranchRefId = BranchId,
                    OpenStatus = 1,
                    OpenDate = DateTime.Now
                }
            };


            var openBranchCommand = new OpenBranchCommand(openBranch);

            var result = await mediator.Send(openBranchCommand);

            if (!result.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Error, result.GetMessage());
            }
            else
            {
                bState.State = true;

                NotificationService.Notify(NotificationSeverity.Success, "Open Branch successful");
            }

        }

        async Task GetCurrencyNotes(int currencyId, int? branchRefId)
        {
            isClicked = !isClicked;

            this.currencyId = currencyId;

            var query = new GetCurrencyCounterQuery(currencyId, branchRefId);

            var rsp = await mediator.Send(query);

            CurrencyNotes = rsp.Items;

        }



        protected async void OnCashVaultOut()
        {
            if (BranchId is null && BranchId <= 0)
                return;

            var param = new Dictionary<string, object> { { "BranchId", BranchId! } };

            var options = new DialogOptions
            {
                ShowTitle = false,
                Width = "100vw",
                Height = "100vh",
            };

            var resultModal = await DialogService.OpenAsync<VaultOutModal>("Vault Out", param, options);

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


//canOpenBranch = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.OpenBranch);

//canCloseBranch = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.CloseBranch);

//canVaultOut = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.TranxVaultOut);

//canVaultIn = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.TranxVaultIn);