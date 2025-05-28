using Applications.Constants.Authorizations;
using Applications.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Applications.Requests.Transactions.Journals;
using Applications.Requests.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Responses.ViewModels.Journals;
using Mapster;
using Domains.Models.Transaction;
using Applications.Extensions.mappers;
using Applications.Responses.Lists.Currency;
using Applications.Responses.Paginations;
using Applications.Services.Currencies;
using Applications.Services.Lists;
using Applications.Services.Transactions.Journals;

namespace TrapchumApp.Components.Pages.Transactions.Journals;

public partial class JournalEntryCreate(
    ICurrencyServiceImpl currencyService,
    ILedgerServiceImpl ledgerService,
    IJournalServiceImpl journalService,
    IOtherNameServiceImpl nameService)
{
    protected JournalEntryDtoCreate JLe = new JournalEntryDtoCreate();
    protected List<JournalLineDtoCreate> JournalLines = [];
    private IEnumerable<CurrencyResponse>? _currencies = [];
    private IEnumerable<JournalTypeDto>? _journalTypes = [];
    protected IEnumerable<VLedger>? Ledgers = [];
    protected IEnumerable<VNameList> NameLists = [];
    private VLedger? _ledger = new();
    private bool _isMessage = false;
    private string _messageAlert = "";
    private string _titleMessage = "";
    private int? _selectedCurrencyId;
    protected string SelectedJournalType = "";
    private EditForm? _editForm;
    [Parameter] public string? UserId { get; set; }
    [Parameter] public int BranchId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            JLe.JournalType = JournalTypes.PV.ToString();
            JLe.JournalDate = DateTime.Now;
            JLe.CurrencyRefId = (int)CurrenciesType.Khr;
            AddNewRow();
            await LoadRelatedData();
            await GetLedgerByCurrency((int)CurrenciesType.Khr);
            await GetNameLists(null);
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            NotificationService!.Notify(NotificationSeverity.Error, "Exception1", ex.Message);
        }
    }

    private async Task LoadRelatedData()
    {
        var currenciesRsponse = await currencyService.GetCurrencies(0);

        if (currenciesRsponse.IsSuccess)
        {
            _currencies = currenciesRsponse.Data!.Maps<CurrencyResponse>();
        }

        var journalTypes = await journalService.GetJournalTypesAsync(null);

        if (journalTypes.Any())
        {
            _journalTypes = journalTypes.Maps<JournalTypeDto>();
        }
    }

    private async Task GetLedgerByCurrency(int? currencyId, string? searchText = null)
    {
        var res = await ledgerService.GetLedgersAsync(0, currencyId, searchText);

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

    private async Task GetNameLists(string? id = null, string? searchText = null)
    {
        var param = new QueryParam(1, 10, string.Empty, true, false, searchText);

        NameLists = await nameService.GetNameLists<VNameList>(id, param);

        StateHasChanged();
    }

    protected void AddNewRow()
    {
        JournalLines!.Add(new JournalLineDtoCreate());
    }

    protected void DeleteEvent(JournalLineDtoCreate journalEntryLine)
    {
        var removedIndex = JournalLines!.IndexOf(journalEntryLine);

        var count = JournalLines.Count();

        if (removedIndex != count - 1)
        {
            return;
        }

        JournalLines!.Remove(journalEntryLine);

        GetTotalValue();
    }

    protected void GetTotalValue()
    {
        JLe.TotalDr = JournalLines?.Sum(x => x.Debit);
        JLe.TotalCr = JournalLines?.Sum(x => x.Credit);
    }

    protected async Task OnSave()
    {
        _visibled = true;

        try
        {
            var trx = new JournalTransactionDtoCreate
            {
                TransactionsId = $"trx-{Guid.CreateVersion7()}",
                TranTypeRefId = 33,
                TransactionTypeName = "Journal Entries",
                CurrencyRefId = 1,
                UserRefId = UserId
            };
            JLe.LoanTransaction = trx;
            JLe.JournalEntryId = $"jrn-{Guid.CreateVersion7()}";
            JLe.BranchRefId = BranchId;
            JLe.BankRefId = 11;
            if (JournalLines.Count() <= 1)
            {
                _isMessage = true;

                _messageAlert = "Cannot save this transaction with one journal line";

                _titleMessage = "Invalid JournalLine: ";

                _visibled = false;

                return;
            }

            foreach (var value in JournalLines!)
            {
                var isTrue = await ValidateNameType(value);

                if (isTrue == false)
                {
                    _visibled = false;

                    return;
                }
            }

            if (JLe.TotalCr != JLe.TotalDr)
            {
                _isMessage = true;

                _messageAlert = "Total credit and debit is not balance.";

                _titleMessage = "Not Balance: ";

                _visibled = false;

                return;
            }

            JLe.TotalCr = JournalLines!.Sum(x => x.Credit);
            JLe.TotalDr = JournalLines!.Sum(x => x.Debit);
            JLe.JournalLineCount = JournalLines!.Count();
            JLe.JournalEntryLines = JournalLines;
            if (_editForm?.EditContext?.Validate() == false)
            {
                return;
            }

            var resp = await journalService.CreateJournalAsync(JLe);

            if (resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Journal", "Success");
                JLe.JournalEntryLines = [];
                JournalLines = [];
                AddNewRow();
                _isMessage = false;
                _visibled = false;
                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Journal", resp.ErrMessage);
                _isMessage = false;
                _visibled = false;
            }
        }
        catch (Exception ex)
        {
            _visibled = false;

            _isMessage = false;

            NotificationService.Notify(NotificationSeverity.Error, "Exception", ex!.Message);
        }
    }

    protected void GetAmount(JournalLineDtoCreate journalLine)
    {
        journalLine.Amount = journalLine.Debit + journalLine.Credit;
    }

    decimal? _savedValue = 0;

    protected void DebitValidate(JournalLineDtoCreate journalLine)
    {
        if (journalLine.Credit > 0)
        {
            journalLine.Debit = journalLine.Credit;

            journalLine.Credit = null;
        }

        if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
        {
            _isMessage = true;

            _messageAlert = "Please select any accounts first";

            _titleMessage = "Missing Account: ";

            return;
        }
        else
        {
            _isMessage = false;

            _messageAlert = "";

            _titleMessage = "";
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
            _isMessage = true;

            _messageAlert = "Please select any accounts first";

            _titleMessage = "Missing Account: ";

            return;
        }
        else
        {
            _isMessage = false;

            _messageAlert = "";

            _titleMessage = "";
        }

        GetTotalValue();

        StateHasChanged();
    }

    int _accApCount = 0;

    protected void ValidateLedgerType(JournalLineDtoCreate journalLine)
    {
        _isFocusCredit = false;

        _isFocusDebit = false;

        if (_ledger == null) return;

        _isMessage = false;

        Console.WriteLine("LedgerID: " + journalLine.LedgerRefId);

        if (_ledger!.LedgerTypeCode!.ToUpper() == "ACCOUNTS PAYABLE")
        {
            //check vendor
            _accApCount++;

            _isMessage = true;

            _messageAlert = "Account that schoose is Accounts Payable, vendor name is required";

            _titleMessage = "Required Name: ";
        }

        if (_ledger!.LedgerTypeCode?.ToUpper() == "ACCOUNTS RECEIVABLE")
        {
            //check customer
            _accApCount++;

            _isMessage = true;

            _messageAlert = "Account that schoose is Accounts Receivable, customer name is required";

            _titleMessage = "Required Name: ";
        }

        if (_accApCount > 1)
        {
            //You cannot use more than A/R or A/P account in the same transaction.
            _isMessage = true;

            _messageAlert = "You cannot use more than A/R or A/P account in the same transaction.";

            _titleMessage = "Invalid Account: ";
        }

        StateHasChanged();
    }

    bool _isFocusDebit = false;

    bool _isFocusCredit = false;

    protected void OnNextPerform(JournalLineDtoCreate journalLine)
    {
        var index = JournalLines!.IndexOf(journalLine);

        if (journalLine.LedgerRefId == 0 || journalLine.LedgerRefId == null)
            return;
        if (index >= 1)
        {
            journalLine.Credit = null;

            journalLine.Debit = null;

            GetTotalValue();

            if (JLe.TotalDr > JLe.TotalCr)
            {
                journalLine.Credit = JLe.TotalDr - JLe.TotalCr;
            }
            else if (JLe.TotalCr > JLe.TotalDr)
            {
                journalLine.Debit = JLe.TotalCr - JLe.TotalDr;
            }

            GetTotalValue();

            StateHasChanged();
        }
    }

    protected void OnGetSavedValue(JournalLineDtoCreate journalLine)
    {
        GetTotalValue();
    }

    VNameList? _nameList;


    protected async Task<bool> ValidateNameType(JournalLineDtoCreate journalLine)
    {
        try
        {
            if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
            {
                _isMessage = true;
                _messageAlert = "Some journal lines are missing account.";
                _titleMessage = "Missing Account: ";
                return false;
            }

            await GetNameLists(journalLine.EntityNameRefId);

            if (NameLists.Any())
            {
                _nameList = NameLists.FirstOrDefault();

                journalLine.EntityName = _nameList?.FullName;
            }
            else
            {
                _nameList = null;
            }

            var resp = await ledgerService!.FindLedgerByIdAsync(journalLine.LedgerRefId);

            if (resp.IsSuccess)
            {
                var ledger = resp.Data;

                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS PAYABLE")
                {
                    _accApCount++;
                    //check vendor
                    if (_nameList?.EntType?.ToUpper() != "VENDOR" || journalLine.EntityNameRefId == null)
                    {
                        _isMessage = true;
                        _messageAlert = "Please choose a vendor when using an Accounts Payable account as a line.";
                        _titleMessage = "Required Name: ";
                        return false;
                    }
                }

                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS RECEIVABLE")
                {
                    //check customer
                    _accApCount++;
                    if (_nameList?.EntType?.ToUpper() != "CUSTOMER" || journalLine.EntityNameRefId == null)
                    {
                        _isMessage = true;
                        _messageAlert = "Please choose a customer when using an Accounts Payable account as a line.";
                        _titleMessage = "Required Name: ";
                        return false;
                    }
                }
            }

            if (_accApCount > 1)
            {
                //You cannot use more than A/R or A/P account in the same transaction.
                _isMessage = true;
                _messageAlert = "You cannot use more than A/R or A/P account in the same transaction.";
                _titleMessage = "Invalid Account: ";
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

    int? _valueSize = 10;

    bool _visibled = false;
    [Parameter] public EventCallback<int> Close { get; set; }

    protected async void CloseSelectedTab()
    {
        await Close.InvokeAsync(0);

        StateHasChanged();
    }

    private async Task SelectedCurrencyChanged(object? obj)
    {
            if (obj is not null)
            {
                
                var item = (CurrencyResponse)obj;

                await GetLedgerByCurrency(item.CurrencyId, null);
                
                JournalLines!.Clear();

            }
            AddNewRow();
            
            StateHasChanged();
    }

    private void OnValidateDate()
    {
        //Check date 
        var day = (DateTime.Now - JLe.JournalDate!.Value).Days;

        if (day > 30)
        {
            _isMessage = true;

            _messageAlert = "This transaction is more than 30 days in the past.";

            _titleMessage = "Past Transaction: ";
        }
        else if (day < -30)
        {
            _isMessage = true;

            _messageAlert = "This transaction is more than 30 days in the future.";

            _titleMessage = "Future Transaction: ";
        }
        else
        {
            _isMessage = false;

            _messageAlert = "";

            _titleMessage = "";
        }

        return;
    }

    private void SelectedItemChanged(object? obj)
    {
        if (obj is not null)
        {
            var item = (JournalTypeDto)obj;
            JLe.JournalType = item?.Code;
        }
    }
}