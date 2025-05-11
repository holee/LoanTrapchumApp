using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Transactions.CashManagements;
using Applications.Responses.Lists.Currency;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;
namespace TrapchumApp.Components.Pages.Transactions.Branches
{
    public partial class VaultInModal
    {
        [Inject] ICurrencyService curService { get; set; } = default!;
        [Inject] ICashVaultService caVaultService { get; set; } = default!;
        
        [Parameter] public string? UserId { get; set; }
        [Parameter] public int BranchId { get; set; }
        VCashVaults? vCashVault = new VCashVaults();
        List<CashManagementDetailDtoCreate> cashMDList = [];
        IEnumerable<VCashVaults>? vCashVaults = [];
        IEnumerable<ViewModelCurrencyNotes>? CurrencyNotes = [];
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
            //NewCashMDVIn = new();
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
            //NewCashMDVIn = new();

        }
        protected void OnGetCurrencyNote(int? currencyId = 1)
        {

            vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == currencyId).FirstOrDefault();

            cashMDList = CurrencyNotes!.CashManagementsDtoCreate(BranchId, currencyId, 11);

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
            var cashVaultCreate = new CashVaultDtoCreate(BranchId, 0, UserId, 7, DateTime.Now);

            var NewCashMDVIn = cashMDList.Where(x => x.Quantity > 0)
                                        .Select(x => new CashManagementDetailDtoCreate
                                          (
                                              BranchId,
                                              0,
                                              x.CurrencyRefId,
                                              x.CurrencyRefName,
                                              x.CurrencyNoteRefId,
                                              x.CurrencyNoteValue,
                                              x.Quantity
                                          )).ToList();

            var cashMgs = new CashManagementDtoCreate
            {
                BankRefId = branch?.BankRefId,
                //StaffRefId = UserId,
                CurrencyRefId = CurrencyId,
                BranchRefId = branch?.BranchId,
                Amount = totalAmount,
                Balance = 0,
                CashStatusRefId = 7,
                TransactionDate = DateTime.Now,
                BusinessDate = DateTime.Now,
                //DelegateToStaffRefId = (int)UserId,
                TmpAmount = 0,
                Remark = "VaultIn",
                CashManagementDetails = NewCashMDVIn,
            };


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

            var tranx = new TransactionCashVaultDtoCreate(Guid.NewGuid(), 78, "", UserId, DateTime.Now, cashVaultCreate, cashMgs);

            var request = tranx.Adapt<LoanTransaction>();

            var result = await caVaultService.CreateCashVaultAsync(request);

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

                // Console.WriteLine(selectedResult.CurrencyId);

            }


        }
    }
}