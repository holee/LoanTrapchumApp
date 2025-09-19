using Applications.Commons.Extensions;
using Applications.Enumerations;
using Applications.Extensions.Enums;
using Applications.Features.CashManagements.Extensions;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Extensions;
using Applications.Features.Currencies.Responses;
using Applications.Features.Entities;
using Applications.Features.Transactions.CashManagements;
using Applications.Responses.Lists.Currency;
using Applications.Services.Transactions.Branches;
using Mapster;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class VaultInModal(ICurrencyServiceImpl curService, ICashVaultServiceImpl caVaultService, IStartVaultInUseCase startVaultInUseCase)
    {

        [Parameter] public int BranchId { get; set; }
        [Parameter] public int BankId { get; set; } = 11;
        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;

        private string? _userId = string.Empty;
        VCashVaults? vCashVault = new VCashVaults();
        List<CashManagementDetailDtoCreate> cashMDList = [];
        IEnumerable<VCashVaults>? vCashVaults = [];
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
            try
            {
                var auth = await AuthState;

                _userId = auth.GetUserId();

                var currencies = await curService.GetCurrencies(0);

                if (currencies.IsSuccess)
                {
                    CurrencyList = currencies.Data.Adapt<IEnumerable<CurrencyResponse>>();

                    Currency = CurrencyList!.FirstOrDefault();

                    CurrencyId = Currency?.CurrencyId;
                }

                var cashVaultResp = await caVaultService.GetViewCashVaultAsync();

                if (cashVaultResp.Any())
                {
                    vCashVaults = cashVaultResp ?? [];

                    userBalance = vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).Sum(x => x.UserBalance);

                }
                var cashNotes = await caVaultService.GetCurrencyNotesAsync(BranchId);

                CurrencyNotes = cashNotes ?? [];


            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            await base.OnInitializedAsync();

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

            vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == currencyId).FirstOrDefault();

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
            var cashVaultCreate = CashVaultDtoCreate.Create(BranchId, BankId, _userId, (int)CashStatusEnum.VAULTIN, DateTime.Now);

            var cashMgs = CashManagementDtoCreate.Create(BranchId, BankId, CurrencyId, totalAmount, 0, (int)CashStatusEnum.VAULTIN, "VaultIn", cashMDList.ToCashValutIn(11, BranchId));

            string confirmText = "Are you sure!\n You want to vault in?\n Total amount";

            var confirmOption = new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" };

            vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).FirstOrDefault();

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

            var tranx = TrnxCashVaultDtoCreate.Create(BranchId, TransactionTypes.CSV, tranName, _userId, cashVaultCreate, cashMgs);

            var result = await startVaultInUseCase.ExecuteAsync(tranx);

            if (!result.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault In", result.ErrMessage);
               
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