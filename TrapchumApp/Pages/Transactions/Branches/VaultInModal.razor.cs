using Applications.Commons.Extensions;
using Applications.Features.Branches.Dtos;
using Applications.Features.CashManagements.Command.CreateCashVaultIn;
using Applications.Features.CashManagements.Extensions;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Extensions;
using Applications.Features.Currencies.Queries.GetCashVault;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetCurrencyNotes;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.CashManagements;
using Domains.Enumerations;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class VaultInModal
    {

        [Parameter] public int BranchId { get; set; }
        [Parameter] public int BankId { get; set; } = 11;
        [PersistentState] public string? UserId { set; get; }
        [PersistentState] public List<CashManagementDetailDtoCreate>? cashMDList { get; set; }
        [PersistentState] public IEnumerable<VCashVaults>? _vCashVaults { get; set; }
        [PersistentState] public IEnumerable<VCurrencyNotes>? CurrencyNotes { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;

        VCashVaults? vCashVault = new VCashVaults();

        BranchDto? branch = new BranchDto();

        decimal? userBalance = 0;

        int? CurrencyId;

        decimal? totalAmount = 0;

        decimal total = 0;

        IEnumerable<CurrencyDto>? _currencyList = [];

        CurrencyDto? _currency = new CurrencyDto();
        protected override async Task OnParametersSetAsync()
        {

            OnGetCurrencyInCounter(CurrencyId);

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

            await base.OnInitializedAsync();

        }

        private async Task GetCurrencies()
        {

            if(_currencyList is not null && _currencyList.Any())
            {
                return;
            }

            var query = new GetCurrenciesDtoQuery(0, new QueryParam(0, 25));

            var rsp = await mediator.Send(query);

            _currencyList = rsp.Items;

            await GetCurrency();

        }

        private async Task GetCurrency()
        {
            var command = new GetCurrencyByIdQuery((int)CurrencyEnum.KHR);

            var rsp1 = await mediator.Send(command);

            if (rsp1.IsSuccess())
            {
                _currency = rsp1.Value;

                CurrencyId = _currency?.CurrencyId;

            }
        }

        private async Task GetCashVaultBalance()
        {
            if(_vCashVaults is null)
            {
                var command = new GetCashVaultQuery(BranchId);

                var rsp1 = await mediator.Send(command);

                _vCashVaults = rsp1.Items ?? [];

                userBalance = GetUserBalance(_vCashVaults);

            }

        }

        private async Task GetCurrencyNotes()
        {
            if(CurrencyNotes is not null && CurrencyNotes.Any())
            {
                return;
            }

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

            OnGetCurrencyInCounter();

            totalAmount = 0;

            total = 0;

            _currency = _currencyList!.Where(x => x.CurrencyId == CurrencyId).FirstOrDefault();

            sp = _currency?.CurrencyId == (int)CurrencyEnum.KHR ? "#,#0" : "#,#0.00";

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
        protected void OnGetCurrencyInCounter(int? currencyId = 1)
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
            if (cashMDList is null || !cashMDList.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault In", "Please add cash note to vault in.");
               
                return;
            }


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

            var command = new StartCashVaultIn(tranx);

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

                var selectedResult = (CurrencyDto)obj;

                OnGetCurrencyInCounter(selectedResult.CurrencyId);

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