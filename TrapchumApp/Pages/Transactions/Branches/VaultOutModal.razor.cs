using Applications.Commons.Extensions;
using Applications.Enumerations;
using Applications.Features.CashManagements.Extensions;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.CashManagements.Responses;
using Applications.Features.CashManagements.UseCases;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.CashManagements;
using Applications.Responses.Lists.Currency;
using Applications.Services.Transactions.Branches;
using Domains.Enumerations;
using TrapchumApp.Helpers.Extensions;
using Applications.Features.Currencies.Extensions;
using Applications.Features.Branches.Dtos;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Responses.Paginations;
using Mapster;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetCurrencyNotes;
namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class VaultOutModal
    {
        private readonly ICashVaultServiceImpl _caVaultService;
        private readonly IStartVaultOutUseCase _startVaultOut;
        private readonly IGetCashVaultUseCase _getCashVault;
        public VaultOutModal(ICashVaultServiceImpl caVaultService, IStartVaultOutUseCase startVaultOut, IGetCashVaultUseCase getCashVault)
        {
            _caVaultService = caVaultService;
            _startVaultOut = startVaultOut;
            _getCashVault = getCashVault;
        }

        [Parameter] public int BranchId { get; set; }
        [Parameter] public int BankId { get; set; } = 11;
        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;
        [PersistentState]public string? UserId { get; set; }

        private IEnumerable<VCashVaults>? vCashVaults = [];

        private IEnumerable<VCurrencyNotes>? CurrencyNotes = [];

        private VCashVaults? vCashVault;

        private TrnxCashVaultDtoCreate transaction = new TrnxCashVaultDtoCreate();

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

            if (string.IsNullOrEmpty(UserId))
            {

                var auth = await AuthState;

                UserId = auth.GetUserId();
            }

            await GetCurrenciesList();

            await GetCurrency((int)CurrencyEnum.KHR);

            await GetCurrencyNotes();

            await base.OnInitializedAsync();
        }

        private async Task GetCurrencyNotes()
        {

            var command = new GetCurrencyNotesQuery(BranchId, null, new QueryParam(0, 0));

            var noteResp = await mediator.Send(command);

            CurrencyNotes = noteResp.Items;

            OnGetCurrencyNote(CurrencyId);

        }

        private async Task GetCurrency(int? currencyId)
        {
            var command = new GetCurrencyByIdQuery(currencyId);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                Currency = rsp.Value.Adapt<CurrencyResponse>();

                CurrencyId = currencyId;
            }

        }

        private async Task GetCurrenciesList()
        {
            var command = new GetCurrenciesQuery(0, new QueryParam(0, 0));

            var rsp = await mediator.Send(command);  //_curService.GetCurrencies<CurrencyResponse>(0);

            CurrencyList = rsp.Items.Adapt<IEnumerable<CurrencyResponse>>();

        }

        string sp = "#,#0";
        private async Task OnSelectChanges(object? arg)
        {
            try
            {
                if (arg is not null)
                {
                    var id = ((CurrencyResponse)arg)?.CurrencyId;

                    CurrencyId = id;

                    Currency = ((CurrencyResponse)arg!);

                    OnGetCurrencyNote(id);

                    totalAmount = 0;

                    total = 0;

                    await GetCurrency(id);
                    sp = Currency?.CurrencyCode == CurrencyEnum.KHR.ToString() ? "#,#0" : "#,#0.00";

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
                cashMDList = CurrencyNotes!.ToCashManagementDetail(currencyId, BranchId, BankId);
            }
            StateHasChanged();
        }


        async Task GetCashVaults()
        {
            var rsp = await _getCashVault.ExecuteAsync(BranchId);

            if (rsp.IsSuccess)
            {
                vCashVaults = rsp.Data ?? [];
            }
        }

        protected async Task OnSaveChangesClose()
        {

            NewCashMDVOut = cashMDList.ToCashVaultOut(11, BranchId);

            if (totalAmount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault Out", "Transaction of vault out was failed!");

                return;
            }


            await GetCashVaults();

            vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).FirstOrDefault();

            if (totalAmount > vCashVault?.VaultBalance)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault Out", "Transaction of vault out was failed!");

                return;
            }


            //Valid vault balance
            bool confirm = (bool)await DialogService!.Confirm($"Are you sure!\n You want to vault out?\n Total amount: {totalAmount}", "Vault Out", new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" });

            if (confirm == false) return;

            //Transaction 

            var trnx = TrnxCashVaultDtoCreate.Create(BranchId, (int)TransactionTypes.CSV,
                                             TransactionTypes.CSV.GetDescription(),
                                             UserId, DateTime.Now);
            //Cash vault

            var cashToVault = CashVaultDtoCreate.Create(BranchId, BankId, UserId, (int)CashStatusEnum.VAULTOUT, DateTime.Now);

            transaction.CashVault = cashToVault;


            //CashManagement 

            decimal? balance = totalAmount + vCashVault!.UserBalance;

            var cashMgn = CashManagementDtoCreate.Create(BranchId, BankId, CurrencyId, totalAmount, balance, (int)Applications.Enumerations.CashStatusEnum.VAULTOUT, "VaultOut", NewCashMDVOut);

            transaction.CashManagements.Add(cashMgn);

            var result = await _startVaultOut.ExecuteAsync(transaction);

            if (result.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Vault Out", "Vault out is succeed!");
                transaction = new();
                DialogService.Close(true);
            }
            else
            {
                CurrencyNotes = new List<VCurrencyNotes>();
                NewCashMDVOut = new();
                NotificationService.Notify(NotificationSeverity.Error, "Vault Out", result.ErrMessage);
            }
        }

        void OnClear()
        {
            total = 0;
            totalAmount = 0;
            CurrencyNotes = [];
            NewCashMDVOut = new();
        }

        private async Task OnSaveChanges()
        {
            var confirmOption = new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" };

            var comfirmMessage = $"Are you sure!\n You want to vault out?\n Total amount: {totalAmount}";

            NewCashMDVOut = cashMDList.ToCashVaultOut(BranchId, BankId);

            if (totalAmount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vault Out", "Amount and vault balance is invalid");

                return;
            }

            await GetCashVaults();

            vCashVault = vCashVaults?.Where(x => x.CurrencyRefId == CurrencyId).FirstOrDefault();

            var confirm = await DialogService!.Confirm(comfirmMessage, "Vault Out", confirmOption);

            // Valid vault balance
            if (totalAmount <= vCashVault?.VaultBalance && confirm == true)
            {

                //Transaction 

                transaction = TrnxCashVaultDtoCreate.Create(BranchId, (int)TransactionTypes.CSV, TransactionTypes.CSV.GetDescription(), UserId);

                //Cash vault
                var cashvault = CashVaultDtoCreate.Create(BranchId, BankId, UserId, (int)CashStatusEnum.VAULTOUT, DateTime.Now);

                transaction.CashVault = cashvault;

                //CashManagement 

                var balance = totalAmount + vCashVault.UserBalance;

                var cashMgn = CashManagementDtoCreate.Create(BranchId, BankId, CurrencyId, totalAmount, balance, (int)CashStatusEnum.VAULTOUT, "VaultOut", NewCashMDVOut);

                transaction.CashManagements.Add(cashMgn);

                var result = await _caVaultService.CreateCashVaultAsync(transaction);

                if (result.IsSuccess == true)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Vault Out", "Vault out is succeed!");

                    transaction = new();

                    OnClear();

                    DialogService.Refresh();

                    await OnInitializedAsync();
                }
                else
                {
                    OnClear();
                    NotificationService.Notify(NotificationSeverity.Success, "Vault Out", result.ErrMessage);
                }
            }


        }


        void Calculate()
        {
            total = cashMDList.Sum(x => x.TotalValue)!.Value;
            totalAmount = total;
        }

    }
}


//Transaction 
//transaction.TranTypeRefId = (int)TransactionTypes.CSV;// 78;
//transaction.TransactionTypeName = TransactionTypes.CSV.GetDescription();
//transaction.UserRefId = _userId;


//Cash vault
//transaction.CashVault!.CashVaultsId = Guid.CreateVersion7().ToString();
//transaction.CashVault!.BranchRefId = BranchId;
//transaction.CashVault.BankRefId = 11;
//transaction.CashVault.UserRefId = _userId;
//transaction.CashVault.CvStatusRefId = (int)CashStatusEnum.VAULTOUT; //8;
//transaction.CashVault.CvDate = DateTime.Now;

//CashManagement 

//transaction.CashManagements.Add(new CashManagementDtoCreate
//{
//    Id = Guid.CreateVersion7().ToString(),
//    BankRefId = 11,
//    CurrencyRefId = CurrencyId,
//    BranchRefId = BranchId,
//    Amount = totalAmount,
//    Balance = totalAmount + vCashVault!.UserBalance,
//    CashStatusRefId = (int)CashStatusEnum.VAULTOUT,
//    TransactionDate = DateTime.Now,
//    BusinessDate = DateTime.Now,
//    Remark = "VaultOut",
//    CashManagementDetails = NewCashMDVOut,
//});