using Applications.Commons.Extensions;
using Applications.Enumerations;
using Applications.Extensions.Enums;
using Applications.Features.Branches.Dtos;
using Applications.Features.CashManagements.Extensions;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Command.CreateCashVaultIn;
using Applications.Features.Currencies.Extensions;
using Applications.Features.Currencies.Queries.GetCashVault;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Currencies.Queries.GetCurrencyNotes;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.CashManagements;
using Applications.Responses.Lists.Currency;
using Applications.Responses.Paginations;
using Applications.Services.Transactions.Branches;
using Mapster;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Branches
{
    //ICurrencyServiceImpl curService, 
    //CashVaultServiceImpl caVaultService,
    //IStartVaultInUseCase startVaultInUseCase
    public partial class VaultInModal
    {

        [Parameter] public int BranchId { get; set; }
        [Parameter] public int BankId { get; set; } = 11;

        [PersistentState]
        public string? UserId { set; get; }
        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;

        VCashVaults? vCashVault = new VCashVaults();
        List<CashManagementDetailDtoCreate> cashMDList = [];
        IEnumerable<VCashVaults>? _vCashVaults = [];
        IEnumerable<VCurrencyNotes>? CurrencyNotes = [];
        BranchDto? branch = new BranchDto();
        decimal? userBalance = 0;
        int? CurrencyId;
        decimal? totalAmount = 0;
        decimal total = 0;
        IEnumerable<CurrencyResponse>? CurrencyList = [];

        CurrencyResponse? Currency = new CurrencyResponse();
        protected override async Task OnParametersSetAsync()
        {

            OnGetCurrencyNote(CurrencyId);

            await base.OnParametersSetAsync();
        }


        protected override async Task OnInitializedAsync()
        {

            if (string.IsNullOrEmpty(UserId))
            {
                var auth = await AuthState;

                UserId = auth.GetUserId();
            }
            await GetCurrencies();

            await GetCashVaultBalance();

            await GetCurrencyNotes();


            //var cashNotes = await caVaultService.GetCurrencyNotesAsync(BranchId);

            //CurrencyNotes = cashNotes ?? [];

            await base.OnInitializedAsync();

        }

        private async Task GetCurrencies()
        {
            var query = new GetCurrenciesQuery(BranchId, new QueryParam(0, 25));

            var rsp = await mediator.Send(query); //curService.GetCurrencies(query);

            CurrencyList = rsp.Items.Adapt<IEnumerable<CurrencyResponse>>();

            Currency = CurrencyList!.FirstOrDefault();

            CurrencyId = Currency?.CurrencyId;


        }

        private async Task GetCashVaultBalance()
        {
            var command = new GetCashVaultQuery(0);

            var rsp1 = await mediator.Send(command);  //caVaultService.GetViewCashVaultAsync();

            if (rsp1.Items.Any())
            {
                _vCashVaults = rsp1.Items ?? [];

                userBalance = GetUserBalance(_vCashVaults);

            }
        }

        private async Task GetCurrencyNotes()
        {
            var command2 = new GetCurrencyNotesQuery(BranchId, null, new QueryParam(0, 0));

            var rsp2 = await mediator.Send(command2);

            CurrencyNotes = rsp2.Items ?? [];
        }

        private decimal? GetUserBalance(IEnumerable<VCashVaults>? x)
        {
            return x?.Where(x => x.CurrencyRefId == CurrencyId).Sum(x => x.UserBalance);
        }

        string sp = "#,#0";

        private void SelectValueChange(int? cid)
        {
            if (cid.HasValue) CurrencyId = cid;

            OnGetCurrencyNote();

            totalAmount = 0;

            total = 0;

            Currency = CurrencyList!.Where(x => x.CurrencyId == CurrencyId).FirstOrDefault();

            if (Currency?.CurrencyCode == "KHR")
                sp = "#,#0";
            else sp = "#,#0.00";

            StateHasChanged();
        }
        void CalTotal()
        {
            total = cashMDList!.Sum(x => x.TotalValue)!.Value;

            totalAmount = total;
        }
        void OnClear()
        {
            total = 0;
            totalAmount = 0;
            CurrencyNotes = [];
        }
        protected void OnGetCurrencyNote(int? currencyId = 1)
        {

            vCashVault = _vCashVaults?.Where(x => x.CurrencyRefId == currencyId).FirstOrDefault();

            cashMDList = CurrencyNotes!.CashManagementsDtoCreate(BranchId, currencyId, BankId);

        }
        protected async Task OnSaveChangesClose()
        {
            await HandleSaveChanges(true);
        }

        protected async Task OnSaveChanges()
        {
            await HandleSaveChanges(false);
        }

        async Task HandleSaveChanges(bool saveClosed = false)
        {
            var cashVaultCreate = CashVaultDtoCreate.Create(BranchId, BankId, UserId, (int)CashStatusEnum.VAULTIN, DateTime.Now);

            var cashMgs = CashManagementDtoCreate.Create(BranchId, BankId, CurrencyId, totalAmount, 0, (int)CashStatusEnum.VAULTIN, "VaultIn", cashMDList.ToCashValutIn(11, BranchId));

            string confirmText = "Are you sure!\n You want to vault in?\n Total amount";

            var confirmOption = new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" };

            vCashVault = _vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).FirstOrDefault();

            if (totalAmount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault In", "Amount is invalid.");

                return;
            }

            if (totalAmount != vCashVault?.UserBalance)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault In", "Amount and user balance is invalid");

                return;
            }

            var confirm = await DialogService!.Confirm($"{confirmText}:{totalAmount}", "Vault In", confirmOption);

            if (confirm == false) return;

            var tranName = TransactionTypes.CSV.GetDescription();

            var tranx = TrnxCashVaultDtoCreate.Create(BranchId, TransactionTypes.CSV, tranName, UserId, cashVaultCreate, cashMgs);

            var command = new CreateCashVaultIn(tranx);

            var result = await mediator.Send(command); //startVaultInUseCase.ExecuteAsync(tranx);

            if (!result.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault In", result.GetMessage());
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, "Vault In", "Vault in is succeed!");

            if (saveClosed)
            {
                DialogService.Close(true);
            }


        }


        void SelectedItemChanged(object obj)
        {
            if (obj is not null)
            {

                var selectedResult = (CurrencyResponse)obj;

                OnGetCurrencyNote(selectedResult.CurrencyId);

                totalAmount = 0;

                total = 0;

                sp = selectedResult.CurrencyId switch
                {
                    1 => "#,#0",
                    2 => "#,#0.00",
                    _ => "#,#0"
                };

                // NewCashMDVIn = new();

            }


        }
    }
}


//var cashMgs = new CashManagementDtoCreate
//{
//    BankRefId = branch?.BankRefId,
//    CurrencyRefId = CurrencyId,
//    BranchRefId = BranchId,
//    Amount = totalAmount,
//    Balance = 0,
//    CashStatusRefId = (int)CashStarusEnum.VAULTIN,
//    TransactionDate = DateTime.Now,
//    BusinessDate = DateTime.Now,
//    TmpAmount = 0,
//    Remark = "VaultIn",
//    CashManagementDetails = cashMDList.ToCashValutIn(11, BranchId),
//};