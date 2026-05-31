using Applications.Exceptions;
using Applications.Features.Branches.Command.CloseBranch;
using Applications.Features.Branches.Command.OpenBranch;
using Applications.Features.Branches.Queries.GetBranchState;
using Applications.Features.Branches.Queries.GetCashVaultBalance;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Queries.GetCurrencyCounter;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.OpenBranches;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class OpenBranch
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;
        [Parameter] public int? BranchId { get; set; } = 0;
        [PersistentState] public string? UserId { get; set; }
        [PersistentState] public IEnumerable<VCurrencyCounter>? _currencyCounter { get; set; }
        [PersistentState] public IEnumerable<VCashVaultBalance>? CashVaultBalances { get; set; }
        OpenBranchTransactionDto? transactionOpenBranch;
        OpenBranchTransactionDto? transactionCloseBranch;
        bool isClicked = false;
        long userRefId = 0;
        int? currencyId = 1;
        private AlertStyle alertStyle;
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                var auth = await AuthenticationState;

                UserId = auth.GetUserId();

                await bState.GetAuthenticatedUser(auth.User);
            }
            try
            {
                await GetCashVault();

                await GetBranchState();
            }
            catch (MediatorException ex)
            {
                _toastService.ShowError(ex.ErrorMessages.First());
            }

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

                await GetCurrencyCounter(currencyId, BranchId);
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

            var confirm = await MsgBox.ConfirmAsync("Are you sure you want to close branch?", "Confirm");

            if (confirm == MsgBoxResult.No)
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

                _toastService.ShowSuccess("Close Branch successful");

            }
            else
            {
                _toastService.ShowError(result.GetMessage());
            }
        }

        async Task OpenSideDialog(int? currencyId)
        {
            await GetCurrencyCounter(currencyId, BranchId);
        }

        async Task SubmitOpenBranch()
        {
            var confirm = await MsgBox.ConfirmAsync("Are you sure you want to open branch?", "Confirm");

            if (confirm == MsgBoxResult.No)
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
                _toastService.ShowError(result.GetMessage());
            }
            else
            {
                bState.State = true;

                _toastService.ShowSuccess("Open Branch successful");

                await Swal.SuccessAsync("Open Branch successful", "Branch has been opened successfully.");

            }

        }

        async Task GetCurrencyCounter(int? currencyId, int? branchRefId)
        {
            isClicked = !isClicked;

            this.currencyId = currencyId;

            var query = new GetCurrencyCounterQuery(currencyId, branchRefId);

            var rsp = await mediator.Send(query);

            _currencyCounter = rsp.Items;

        }



        protected async void OnCashVaultOut()
        {
            if (BranchId is null && BranchId <= 0)
                return;

            var param = new Dictionary<string, object?> { { "BranchId", BranchId! } };

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