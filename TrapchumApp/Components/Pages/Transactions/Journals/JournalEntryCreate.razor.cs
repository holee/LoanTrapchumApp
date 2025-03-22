using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Applications.Requests.Transactions.Journals;
using Applications.Requests.Lists.Currency;
using Applications.Requests.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Mapster;
using Domains.Models.Transaction;
using Applications.Extensions.mappers;
namespace TrapchumApp.Components.Pages.Transactions.Journals;
public partial class JournalEntryCreate
{
    protected JournalEntryDtoCreate jLe = new JournalEntryDtoCreate();
    protected List<JournalLineDtoCreate> JournalLines = new List<JournalLineDtoCreate>();
    protected IEnumerable<CurrencyDto>? Currencies { get; set; }
    protected IEnumerable<JournalTypeDto>? JournalTypes { get; set; }
    protected IEnumerable<VLedger>? Ledgers = [];
    protected IEnumerable<VNameList> nameLists = [];
    protected IEnumerable<EntityNameVm?>? EntityNames { get; set; }
    protected VLedger? ledger = new();
    private bool IsMessage = false;
    private string MessageAlert = "";
    private string TitleMessage = "";
    protected int? selectedCurrencyId;
    protected string selectedJournalType = "";
    protected EditForm? editForm;
    private readonly ICurrencyService cuService;
    private readonly ILedgerService lgService;
    private readonly IJournalService jnService;
    [Parameter] public string? UserId { get; set; }
    [Parameter] public int BranchId { get; set; }
    protected override async Task OnInitializedAsync()
    {
        try
        {
            selectedCurrencyId = 1;
            jLe.JournalType = "PV";
            jLe.JournalDate = DateTime.Now;
            AddNewRow();
            await LoadRelatedData();
            await GetLedgerByCurrency(selectedCurrencyId);
            await GetNameLists();
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            NotificationService!.Notify(NotificationSeverity.Error, "Exception1", ex.Message);
        }


    }
    private async Task LoadRelatedData()
    {

        var currenciesRsponse = await cuService.GetCurrencies(0);

        if (currenciesRsponse.IsSuccess)
        {
            Currencies = currenciesRsponse.Data!.Maps<CurrencyDto>();
        }

        var journalTypes = await jnService.FindJournalTypesAsync(null);

        if (journalTypes.Any())
        {
            JournalTypes = journalTypes.Maps<JournalTypeDto>();
        }


    }

    protected async Task GetLedgerByCurrency(int? currencyId, string? searchText = null)
    {

        var res = await lgService.GetLedgersAsync(0, currencyId, searchText);

        if (res.IsSuccess)
        {
            Ledgers = res.Data;
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, res.ErrMessage);
        }
        StateHasChanged();
    }

    protected async Task GetNameLists(long id = 0, string? searchText = null)
    {
        nameLists = await jnService.FindNameLists(id, 50, searchText);
        StateHasChanged();
    }

    protected void AddNewRow()
    {
        jLe.JournalEntryLines!.Add(new JournalLineDtoCreate());
    }
    protected void DeleteEvent(JournalLineDtoCreate journalEntryLine)
    {
        var removedIndex = jLe.JournalEntryLines!.IndexOf(journalEntryLine);
        var count = jLe.JournalEntryLines.Count();
        if (removedIndex != count - 1)
        {
            return;
        }
        jLe.JournalEntryLines!.Remove(journalEntryLine);
        GetTotalValue();
    }

    protected void GetTotalValue()
    {
        jLe.TotalDr = jLe.JournalEntryLines?.Sum(x => x.Debit);
        jLe.TotalCr = jLe.JournalEntryLines?.Sum(x => x.Credit);

    }

    protected async Task OnSave()
    {
        visibled = true;
        try
        {
            //var state = await _stateProvider.GetAuthenticationStateAsync();
            //UserId = state.User.GetId();
            //BranchId = state.User.GetBranchId();
            jLe.BranchRefId = BranchId;
            jLe.LoanTransaction!.TranTypeRefId = 33;
            jLe.LoanTransaction.TransactionTypeName = "Journal Entries";
            jLe.LoanTransaction.CurrencyRefId = selectedCurrencyId;
            jLe.LoanTransaction.UserRefId = UserId;
            jLe.BankRefId = 11;
            jLe.CurrencyRefId = selectedCurrencyId;
            jLe.TotalCr = jLe.JournalEntryLines!.Sum(x => x.Credit);
            jLe.TotalDr = jLe.JournalEntryLines!.Sum(x => x.Debit);
            jLe.JournalLineCount = jLe.JournalEntryLines!.Count();
            editForm?.EditContext?.Validate();
            if (jLe.JournalEntryLines!.Count() <= 1)
            {
                IsMessage = true;
                MessageAlert = "Cannot save this transaction with one journal line";
                TitleMessage = "Invalid JournalLine: ";
                visibled = false;
                return;
            }

            foreach (var value in jLe.JournalEntryLines!)
            {

                var isTrue = await ValidateNameType(value);
                if (isTrue == false)
                {
                    visibled = false;
                    Console.WriteLine("Error.....");
                    return;
                }

            }
            if (jLe.TotalCr != jLe.TotalDr)
            {
                IsMessage = true;
                MessageAlert = "Total credit and debit is not balance.";
                TitleMessage = "Not Balance: ";
                visibled = false;
                return;
            }


            var tranx = jLe.Adapt<LoanTransaction>();

            var resp = await jnService.CreateJournalAsync(tranx);

            if (resp.IsSuccess)
            {

                NotificationService.Notify(NotificationSeverity.Success, "Create Journal", "");
                jLe.JournalEntryLines = [];
                AddNewRow();
                IsMessage = false;
                visibled = false;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Create Journal", resp.ErrMessage);
                IsMessage = false;
                visibled = false;
            }

        }
        catch (Exception ex)
        {
            visibled = false;
            IsMessage = false;

            NotificationService.Notify(NotificationSeverity.Error, "Exception", ex!.Message);

        }

    }
    protected void GetAmount(JournalLineDtoCreate journalLine)
    {
        journalLine.Amount = journalLine.Debit + journalLine.Credit;
    }
    decimal? SavedValue = 0;
    protected void DebitValidate(JournalLineDtoCreate journalLine)
    {
        if (journalLine.Credit > 0)
        {
            journalLine.Debit = journalLine.Credit;
            journalLine.Credit = null;

        }
        if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
        {
            IsMessage = true;
            MessageAlert = "Please select any accounts first";
            TitleMessage = "Missing Account: ";
            return;
        }
        else
        {
            IsMessage = false;
            MessageAlert = "";
            TitleMessage = "";
        }
        GetTotalValue();
        StateHasChanged();

    }
    protected void CreditValidate(JournalLineDtoCreate journalLine)
    {

        if (journalLine.Debit > 0)
        {
            journalLine.Credit = journalLine.Debit;
            journalLine.Debit = null;

        }
        if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
        {
            IsMessage = true;
            MessageAlert = "Please select any accounts first";
            TitleMessage = "Missing Account: ";
            return;
        }
        else
        {
            IsMessage = false;
            MessageAlert = "";
            TitleMessage = "";
        }
        GetTotalValue();
        StateHasChanged();
    }
    int AccAPCount = 0;

    protected void ValidateLedgerType(JournalLineDtoCreate journalLine)
    {


        isFocusCredit = false;
        isFocusDebit = false;
        if (ledger == null)
            return;
        IsMessage = false;
        //journalLine.LedgerRefId = ledger!.LedgerId;
        ////ledgerId = ledger!.LedgerId;
        ////ledgerName= ledger?.LedgerName;
        //journalLine.LedgerName= ledger?.LedgerName;

        Console.WriteLine("LedgerID: " + journalLine.LedgerRefId);
        if (ledger!.LedgerTypeCode!.ToUpper() == "ACCOUNTS PAYABLE")
        {
            //check vendor
            AccAPCount++;
            IsMessage = true;
            MessageAlert = "Account that schoose is Accounts Payable, vendor name is required";
            TitleMessage = "Required Name: ";
        }
        if (ledger!.LedgerTypeCode?.ToUpper() == "ACCOUNTS RECEIVABLE")
        {
            //check customer
            AccAPCount++;
            IsMessage = true;
            MessageAlert = "Account that schoose is Accounts Receivable, customer name is required";
            TitleMessage = "Required Name: ";

        }
        if (AccAPCount > 1)
        {
            //You cannot use more than A/R or A/P account in the same transaction.
            IsMessage = true;
            MessageAlert = "You cannot use more than A/R or A/P account in the same transaction.";
            TitleMessage = "Invalid Account: ";
        }
        StateHasChanged();
    }
    bool isFocusDebit = false;
    bool isFocusCredit = false;
    protected void OnNextPerform(JournalLineDtoCreate journalLine)
    {
        var index = jLe.JournalEntryLines!.IndexOf(journalLine);
        if (journalLine.LedgerRefId == 0 || journalLine.LedgerRefId == null)
            return;
        if (index >= 1)
        {
            journalLine.Credit = null;
            journalLine.Debit = null;
            GetTotalValue();
            if (jLe.TotalDr > jLe.TotalCr)
            {
                journalLine.Credit = jLe.TotalDr - jLe.TotalCr;
            }
            else if (jLe.TotalCr > jLe.TotalDr)
            {
                journalLine.Debit = jLe.TotalCr - jLe.TotalDr;

            }
            GetTotalValue();

            StateHasChanged();
        }
    }

    protected void OnGetSavedValue(JournalLineDtoCreate journalLine)
    {
        GetTotalValue();

    }
    VNameList? nameList;

    public JournalEntryCreate(ICurrencyService currencyService, ILedgerService ledgerService, IJournalService journalService)
    {
        cuService = currencyService;
        lgService = ledgerService;
        jnService = journalService;
    }

    protected async Task<bool> ValidateNameType(JournalLineDtoCreate journalLine)
    {
        try
        {
            if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
            {
                IsMessage = true;
                MessageAlert = "Some journal lines are missing account.";
                TitleMessage = "Missing Account: ";
                return false;
            }

            var names = await jnService!.FindNameLists(journalLine.EntityNameRefId);
            if (names.Any())
            {
                nameList = names.FirstOrDefault();
                journalLine.EntityName = nameList?.FullName;
            }
            else
            {
                nameList = null;
            }

            var resp = await lgService!.FindLedgerByIdAsync(journalLine.LedgerRefId);

            if (resp.IsSuccess)
            {

                var ledger = resp.Data;

                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS PAYABLE")
                {
                    AccAPCount++;
                    //check vendor
                    if (nameList?.EntType?.ToUpper() != "VENDOR" || journalLine.EntityNameRefId == null)
                    {
                        IsMessage = true;
                        MessageAlert = "Please choose a vendor when using an Accounts Payable account as a line.";
                        TitleMessage = "Required Name: ";
                        return false;
                    }

                }
                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS RECEIVABLE")
                {
                    //check customer
                    AccAPCount++;
                    if (nameList?.EntType?.ToUpper() != "CUSTOMER" || journalLine.EntityNameRefId == null)
                    {
                        IsMessage = true;
                        MessageAlert = "Please choose a customer when using an Accounts Payable account as a line.";
                        TitleMessage = "Required Name: ";
                        return false;
                    }
                }


            }
            if (AccAPCount > 1)
            {
                //You cannot use more than A/R or A/P account in the same transaction.
                IsMessage = true;
                MessageAlert = "You cannot use more than A/R or A/P account in the same transaction.";
                TitleMessage = "Invalid Account: ";
                return false;
            }

            await InvokeAsync(StateHasChanged);
            return true;
        }
        catch
        {
            return true;

        }

    }
    protected void OnValidateDate()
    {
        //Check date 
        var day = (DateTime.Now - jLe.JournalDate!.Value).Days;
        Console.WriteLine(day.ToString());
        if (day > 30)
        {
            IsMessage = true;
            MessageAlert = "This transaction is more than 30 days in the past.";
            TitleMessage = "Past Transaction: ";

        }
        else if (day < -30)
        {
            IsMessage = true;
            MessageAlert = "This transaction is more than 30 days in the future.";
            TitleMessage = "Future Transaction: ";
        }
        else
        {
            IsMessage = false;
            MessageAlert = "";
            TitleMessage = "";
        }

        return;
    }

}
