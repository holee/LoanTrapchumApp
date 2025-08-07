using Applications.Enumerations;
using Applications.Features.Currencies;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Transactions.CashManagements;
using Applications.Responses.Lists.Currency;
using Applications.Responses.ViewModels.Currencies;
using Applications.Services.Transactions.Branches;
using Domains.Models.Transaction;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class VaultOutModal(ICurrencyServiceImpl curService, ICashVaultServiceImpl caVaultService)
    {
        [Parameter] public int BranchId { get; set; }
        [Parameter] public string? UserId { get; set; }

        private IEnumerable<VCashVaults>? vCashVaults;

        private IEnumerable<ViewModelCurrencyNotes>? CurrencyNotes;

        private VCashVaults? vCashVault;

        private TransactionCashVaultDtoCreate transaction = new TransactionCashVaultDtoCreate();

        private List<CashManagementDetailDtoCreate> cashMDList = [];

        private List<CashManagementDetailDtoCreate> NewCashMDVOut = [];

        private BranchDto? branch = new BranchDto();

        private int? CurrencyId;

        private decimal? totalAmount = 0;

        private decimal? total = 0;

        private string? messageAlert;

        private bool visible = false;
        private IEnumerable<CurrencyResponse>? CurrencyList { get; set; }
        private CurrencyResponse? Currency { get; set; } = new CurrencyResponse();

        protected override async Task OnInitializedAsync()
        {
            var rsp = await curService!.GetCurrencies<CurrencyResponse>(0);

            if (rsp.IsSuccess)
            {
                CurrencyList = rsp.Data;

                Currency = rsp.Data?.FirstOrDefault();
            }

            CurrencyId = (int?)CurrenciesType.Khr;

            var resultResp = await caVaultService!.GetCurrencyNotesAsync(BranchId);

            if (resultResp.Any())
            {
                CurrencyNotes = resultResp ?? [];

                OnGetCurrencyNote(CurrencyId);
            }

            await base.OnInitializedAsync();
        }

        string sp = "#,#0";

        private async Task OnSelectChanges(object? arg)
        {
            try
            {
                if (arg is not null)
                {
                    var id = ((CurrencyResponse)arg)?.CurrencyId;

                    CurrencyId= id;

                    Currency = ((CurrencyResponse)arg!);

                    OnGetCurrencyNote(id);

                    totalAmount = 0;

                    total = 0;

                    var currency = await curService.GetCurrencyById(id);

                    sp = Currency?.CurrencyCode == "KHR" ? "#,#0" : "#,#0.00";
                }

                NewCashMDVOut = new();

                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                NotificationService.Notify(NotificationSeverity.Error, e.Message);
            }
        }

        private void OnGetCurrencyNote(int? currencyId)
        {
            if (CurrencyId.HasValue)
            {
                cashMDList = CurrencyNotes!.Where(x => x.CurrencyRefId == currencyId).Select(x =>
                    new CashManagementDetailDtoCreate
                    {
                        BranchRefId = BranchId,
                        BankRefId = branch?.BankRefId,
                        CurrencyRefId = x.CurrencyRefId,
                        CurrencyRefName = x.CurrencyCode,
                        CurrencyNoteRefId = x.Id,
                        CurrencyNoteValue = (int?)x.CurrencyValue,
                        QOnHand = x.Quantity == null ? 0 : x.Quantity,
                        TotalValue = 0
                    }).ToList();
                    //.Where(x => x.QOnHand > 0).ToList();
            }
            StateHasChanged();
        }


        async Task GetCashVaults()
        {
            var cashVaultResp = await caVaultService.GetCashVaultsAsync(BranchId);

            if (cashVaultResp.IsSuccess)
            {
                vCashVaults = cashVaultResp.Data ?? [];
            }
        }

        protected async Task OnSaveChangesClose()
        {
            NewCashMDVOut = cashMDList.Select(x => new CashManagementDetailDtoCreate
            {
                BranchRefId = branch?.BranchId,
                BankRefId = branch?.BankRefId,
                CurrencyRefId = x.CurrencyRefId,
                CurrencyRefName = x.CurrencyRefName,
                CurrencyNoteRefId = x.CurrencyNoteRefId,
                CurrencyNoteValue = x.CurrencyNoteValue,
                Quantity = x.Quantity,
                TotalValue = x.TotalValue
            }).Where(x => x.Quantity!.Value < 0).ToList();
            try
            {
                if (totalAmount > 0)
                {
                    await GetCashVaults();

                    vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).FirstOrDefault();

                    if (totalAmount <= vCashVault?.VaultBalance)
                    {
                        //Valid vault balance
                        bool confirm = (bool)await DialogService!.Confirm(
                            $"Are you sure!\n You want to vault out?\n Total amount: {totalAmount}", "Vault Out",
                            new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" });
                        if (confirm)
                        {
                            //Transaction 
                            transaction.TranTypeRefId = 78;
                            transaction.TransactionTypeName = "CashVault";
                            transaction.UserRefId = UserId;

                            //Cash vault
                            transaction.CashVault!.BranchRefId = branch?.BranchId;
                            transaction.CashVault.BankRefId = branch?.BankRefId;
                            transaction.CashVault.UserRefId = UserId;
                            transaction.CashVault.CvStatusRefId = 8;
                            transaction.CashVault.CvDate = DateTime.Now;

                            //CashManagement 
                            transaction.CashManagements.Add(new CashManagementDtoCreate
                            {
                                BankRefId = branch?.BankRefId,
                                CurrencyRefId = CurrencyId,
                                BranchRefId = branch?.BranchId,
                                Amount = totalAmount,
                                Balance = totalAmount + vCashVault.UserBalance,
                                CashStatusRefId = 8,
                                TransactionDate = DateTime.Now,
                                BusinessDate = DateTime.Now,
                                DelegateToStaffRefId = 0,
                                TmpAmount = 0,
                                Remark = "VaultOut",
                                CashManagementDetails = NewCashMDVOut,
                            });

                            var tranx = transaction.Adapt<LoanTransaction>();

                            var result = await caVaultService.CreateCashVaultAsync(transaction);

                            if (result.IsSuccess)
                            {
                                NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                                    "Vault out is succeed!");
                                transaction = new();
                                DialogService.Close(true);
                            }
                            else
                            {
                                CurrencyNotes = new List<ViewModelCurrencyNotes>();
                                NewCashMDVOut = new();
                                NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                                    result.ErrMessage);
                            }
                        }
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                            "Amount and vault balance is invalid");
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                    "Transaction of vault out was failed!");

                Console.WriteLine(ex.Message);
            }
        }

        void OnClear()
        {
            total = 0;
            totalAmount = 0;
            CurrencyNotes = new List<ViewModelCurrencyNotes>();
            NewCashMDVOut = new();
        }

        private async Task OnSaveChanges()
        {
            var confirmOption = new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" };
            var comfirmMessage = $"Are you sure!\n You want to vault out?\n Total amount: {totalAmount}";
            NewCashMDVOut = cashMDList.Select(x => new CashManagementDetailDtoCreate
            {
                BranchRefId = branch?.BranchId,
                BankRefId = branch?.BankRefId,
                CurrencyRefId = x.CurrencyRefId,
                CurrencyRefName = x.CurrencyRefName,
                CurrencyNoteRefId = x.CurrencyNoteRefId,
                CurrencyNoteValue = x.CurrencyNoteValue,
                //Quantity = Utility.ToNegative(x.Quantity)
                // TotalValue = Utility.ToNegative(x.TotalValue)
            }).Where(x => x.Quantity!.Value < 0).ToList();
            try
            {
                if (totalAmount > 0)
                {
                    await GetCashVaults();

                    vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).FirstOrDefault();

                    if (totalAmount <= vCashVault?.VaultBalance)
                    {
                        // Valid vault balance
                        var confirm = await DialogService!.Confirm(comfirmMessage, "Vault Out", confirmOption);
                        if (confirm == true)
                        {
                            //Transaction 
                            transaction.TranTypeRefId = 78;

                            transaction.TransactionTypeName = "CashVault";

                            transaction.UserRefId = UserId!;

                            //Cash vault
                            transaction.CashVault.BranchRefId = branch?.BranchId;

                            transaction.CashVault.BankRefId = branch?.BankRefId;

                            transaction.CashVault.UserRefId = UserId;

                            transaction.CashVault.CvStatusRefId = 8;

                            transaction.CashVault.CvDate = DateTime.Now;

                            //CashManagement 
                            transaction.CashManagements.Add(new CashManagementDtoCreate
                            {
                                BankRefId = branch?.BankRefId,
                                //StaffRefId = UserId,
                                CurrencyRefId = CurrencyId,
                                BranchRefId = branch?.BranchId,
                                Amount = totalAmount,
                                Balance = totalAmount + vCashVault.UserBalance,
                                CashStatusRefId = 8,
                                TransactionDate = DateTime.Now,
                                BusinessDate = DateTime.Now,
                                DelegateToStaffRefId = 0,
                                TmpAmount = 0,
                                Remark = "VaultOut",
                                CashManagementDetails = NewCashMDVOut,
                            });

                            var result = await caVaultService.CreateCashVaultAsync(transaction);

                            if (result.IsSuccess == true)
                            {
                                NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                                    "Vault out is succeed!");

                                transaction = new();
                                OnClear();
                                DialogService.Refresh();
                                await OnInitializedAsync();
                            }
                            else
                            {
                                OnClear();
                                NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                                    result.ErrMessage);
                            }
                        }
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                            "Amount and vault balance is invalid");
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Vault Out",
                    "Transaction of vault out was failed!");
                Console.WriteLine(ex.Message);
            }
        }


        void ShowMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                visible = false;
            }
            else
            {
                visible = true;
                messageAlert = message;
            }
        }

        void Calculate()
        {
            total = cashMDList.Sum(x => x.TotalValue)!.Value;
            totalAmount = total;
        }

        void SelectedItemChanged(object obj)
        {
            if (obj is not null)
            {
                var selectedResult = (CurrencyResponse)obj;
                Console.WriteLine(CurrencyId);
            }
        }
    }
}