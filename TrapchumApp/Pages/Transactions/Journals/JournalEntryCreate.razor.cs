using Applications.Extensions.mappers;
using Applications.Helpers;
using Applications.Requests.Journals;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Services.Lists;
using Applications.Services.Transactions.Journals;
using Applications.Features.Transactions.Journals;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Journals.Command.CreateJournal;
using Applications.Features.Currencies.Dto;

namespace TrapchumApp.Pages.Transactions.Journals;

public partial class JournalEntryCreate(
    ILedgerServiceImpl ledgerService,
    IJournalServiceImpl journalService,
    IOtherNameServiceImpl nameService)
{
    protected JournalEntryDtoCreate JLe = new();
    protected List<JournalLineDtoCreate> JournalLines = [];
    private IEnumerable<CurrencyDto>? _currencies = [];
    private IEnumerable<JournalTypeDto>? _journalTypes = [];
    protected IEnumerable<VLedger>? Ledgers = [];
    protected IEnumerable<VNameList> NameLists = [];
    private bool _isMessage;
    private string _messageAlert = "";
    private string _titleMessage = "";
    private int? _selectedCurrencyId;
    private EditForm? _editForm;
    private bool _visibled;
    private string _filter = "";

    [Parameter] public string? UserId { get; set; }
    [Parameter] public int BranchId { get; set; }

    // Computed properties for balance display
    protected decimal TotalDebit => JournalLines.Sum(x => x.Debit ?? 0);
    protected decimal TotalCredit => JournalLines.Sum(x => x.Credit ?? 0);
    protected decimal Difference => Math.Abs(TotalDebit - TotalCredit);
    protected bool IsBalanced => TotalDebit == TotalCredit && TotalDebit > 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            JLe.JournalType = JournalTypes.PV.ToString();
            JLe.JournalDate = DateTime.Now;
            JLe.CurrencyRefId = (int)CurrenciesType.Khr;

            InitializeLines();

            await LoadRelatedData();
            await GetLedgerByCurrency((int)CurrenciesType.Khr);

            _selectedCurrencyId = (int)CurrenciesType.Khr;

            await GetNameLists(null);
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            NotificationService!.Notify(NotificationSeverity.Error, "Error", ex.Message);
        }
    }

    private void InitializeLines()
    {
        JournalLines.Clear();
        for (int i = 1; i <= 6; i++)
        {
            JournalLines.Add(new JournalLineDtoCreate { LineSeq = i });
        }
    }

    private async Task LoadRelatedData()
    {
        var command = new GetCurrenciesQuery(0, new QueryParam(0, 0));
        var rsp = await mediator.Send(command);
        _currencies = rsp.Items.Maps<CurrencyDto>();

        var journalTypes = await journalService.GetJournalTypesAsync(null);
        if (journalTypes.Any())
        {
            _journalTypes = journalTypes.Maps<JournalTypeDto>();
        }
    }

    private async Task GetLedgerByCurrency(int? currencyId, string? searchText = null)
    {
        try
        {
            var res = await ledgerService.GetLedgersAsync(0, currencyId, searchText);
            if (res.IsSuccess())
            {
                Ledgers = res.Value;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, res.GetMessage());
            }
            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
        }
    }

    async Task LoadDataLedger(LoadDataArgs arg)
    {
        if (arg is not null && !string.IsNullOrEmpty(arg.Filter))
        {
            _filter = arg.Filter;
        }
        await GetLedgerByCurrency(_selectedCurrencyId, _filter);
    }

    async Task LoadNamesList(LoadDataArgs arg)
    {
        await GetNameLists(null, arg.Filter ?? string.Empty);
    }

    private async Task GetNameLists(string? id = null, string? searchText = null)
    {
        try
        {
            var param = new QueryParam(1, 10, string.Empty, true, false, searchText);

            NameLists = await nameService.GetNameLists<VNameList>(id, param);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService!.Notify(NotificationSeverity.Error, "Error", ex.Message);
        }
    }

    // Row management
    protected void AddLine()
    {
        var seq = JournalLines.Count + 1;

        JournalLines.Add(new JournalLineDtoCreate { LineSeq = seq });
    }

    protected void DeleteLine(JournalLineDtoCreate line)
    {
        JournalLines.Remove(line);

        ResetLineSequence();

        if (JournalLines.Count == 0)
        {
            JournalLines.Add(new JournalLineDtoCreate { LineSeq = 1 });
        }
    }

    private void ResetLineSequence()
    {
        for (int i = 0; i < JournalLines.Count; i++)
        {
            JournalLines[i].LineSeq = i + 1;
        }
    }

    private void AutoAddRowIfLast(JournalLineDtoCreate row)
    {
        if (row == JournalLines.Last())
        {
            AddLine();
        }
    }

    // Selection handlers
    private void OnAccountSelected(object? obj, JournalLineDtoCreate row)
    {
        if (obj is VLedger ledger)
        {
            row.LedgerName = ledger.LedgerName!;

            row.LedgerRefId = ledger.LedgerId;

            var rowIndex = JournalLines.IndexOf(row);

            if (rowIndex >= 1)
            {
                AutoBalanceLine(row);
            }

            AutoAddRowIfLast(row);

            ClearAlert();
        }
    }

    private void OnNameSelected(object? arg, JournalLineDtoCreate row)
    {
        if (arg is VNameList name)
        {
            row.EntityName = name.AltName;
        }
    }

    private void OnAmountChanged(JournalLineDtoCreate row)
    {
        if (row.Debit > 0)
        {
            row.Credit = null;
        }
        else if (row.Credit > 0)
        {
            row.Debit = null;
        }

        AutoAddRowIfLast(row);

        StateHasChanged();
    }

    private void AutoBalanceLine(JournalLineDtoCreate row)
    {
        row.Credit = null;
        row.Debit = null;

        var otherDebit = JournalLines.Where(x => x != row).Sum(x => x.Debit ?? 0);
        var otherCredit = JournalLines.Where(x => x != row).Sum(x => x.Credit ?? 0);

        if (otherDebit > otherCredit)
        {
            row.Credit = otherDebit - otherCredit;
        }
        else if (otherCredit > otherDebit)
        {
            row.Debit = otherCredit - otherDebit;
        }

        StateHasChanged();
    }

    // Currency change
    private async Task SelectedCurrencyChanged(object? obj)
    {
        if (obj is CurrencyDto item)
        {
            await GetLedgerByCurrency(item.CurrencyId, _filter);
            _selectedCurrencyId = item.CurrencyId;
            JournalLines.Clear();
        }

        InitializeLines();
        StateHasChanged();
    }

    // Date validation
    private void OnValidateDate()
    {
        var day = (DateTime.Now - JLe.JournalDate!.Value).Days;

        if (day > 30)
        {
            ShowAlert("Past Transaction", "This transaction is more than 30 days in the past.");
        }
        else if (day < -30)
        {
            ShowAlert("Future Transaction", "This transaction is more than 30 days in the future.");
        }
        else
        {
            ClearAlert();
        }
    }

    // Save
    protected async Task HandleFormSave()
    {
        _visibled = true;

        if (!JournalLines.Any())
        {
            _visibled = false;
            _messageAlert = "No journal lines to save.";
            ShowAlert("No Journal Lines", _messageAlert);
            return;
        }

        try
        {
            var validLines = JournalLines
                .Where(x => x.LedgerRefId is not null and > 0 && (x.Credit > 0 || x.Debit > 0))
                .ToList();

            if (validLines.Count <= 1)
            {
                ShowAlert("Invalid JournalLine", "Cannot save this transaction with one journal line.");
                _visibled = false;
                return;
            }

            int accApCount = 0;

            foreach (var line in validLines)
            {
                var result = await ValidateNameType(line, accApCount);

                if (!result.IsValid)
                {
                    _visibled = false;

                    return;
                }

                accApCount = result.AccApCount;
            }

            if (!IsBalanced)
            {
                ShowAlert("Not Balanced", "Total credit and debit is not balanced.");

                _visibled = false;

                return;
            }

            var trx = new JournalTransactionDtoCreate
            {
                TranTypeRefId = 33,
                TransactionTypeName = TransactionTypes.JORNL.GetDescription(),
                CurrencyRefId = JLe.CurrencyRefId,
                UserRefId = UserId,
                BranchRefId = BranchId
            };

            JLe.BranchRefId = BranchId;
            JLe.BankRefId = 11;
            JLe.TotalCr = TotalCredit;
            JLe.TotalDr = TotalDebit;
            JLe.JournalLineCount = validLines.Count;
            JLe.JournalEntryLines = validLines;
            trx.JournalEntry = JLe;

            var confirm = await DialogService.Confirm("Are you sure you want to save this journal entry?", "Confirm Save", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

            if(!confirm.HasValue || !confirm.Value)
            {
                _visibled = false;
                return;
            }

            var command = new CreateJournalCommand(trx);

            var resp = await mediator.Send(command);

            if (resp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "Journal", "Journal Entry created successfully!");

                ClearForm();

                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Journal", resp.GetMessage());
            }
        }
        catch
        {
            // Silently handle unexpected errors
        }
        finally
        {
            _visibled = false;

            ClearAlert();
        }
    }

    private async Task<(bool IsValid, int AccApCount)> ValidateNameType(JournalLineDtoCreate journalLine, int accApCount)
    {
        try
        {
            if (journalLine.LedgerRefId is null or 0)
            {
                ShowAlert("Missing Account", "Some journal lines are missing an account.");
                return (false, accApCount);
            }

            VNameList? nameItem = null;
            await GetNameLists(journalLine.EntityNameRefId);
            if (NameLists.Any())
            {
                nameItem = NameLists.FirstOrDefault();
                journalLine.EntityName = nameItem?.FullName;
            }

            var resp = await ledgerService!.FindLedgerByIdAsync(journalLine.LedgerRefId);
            if (resp.IsSuccess())
            {
                var ledger = resp.Value;
                var typeCode = ledger?.LedgerType?.LedgerTypeCode?.ToUpper();

                if (typeCode == "ACCOUNTS PAYABLE")
                {
                    accApCount++;
                    if (nameItem?.EntType?.ToUpper() != "VENDOR" || journalLine.EntityNameRefId == null)
                    {
                        ShowAlert("Required Name", "Please choose a vendor when using an Accounts Payable account.");
                        return (false, accApCount);
                    }
                }

                if (typeCode == "ACCOUNTS RECEIVABLE")
                {
                    accApCount++;
                    if (nameItem?.EntType?.ToUpper() != "CUSTOMER" || journalLine.EntityNameRefId == null)
                    {
                        ShowAlert("Required Name", "Please choose a customer when using an Accounts Receivable account.");
                        return (false, accApCount);
                    }
                }
            }

            if (accApCount > 1)
            {
                ShowAlert("Invalid Account", "You cannot use more than one A/R or A/P account in the same transaction.");
                return (false, accApCount);
            }

            await InvokeAsync(StateHasChanged);
            return (true, accApCount);
        }
        catch
        {
            return (true, accApCount);
        }
    }

    // Clear & Close
    protected void ClearForm()
    {
        JLe = new JournalEntryDtoCreate
        {
            JournalType = JournalTypes.PV.ToString(),
            JournalDate = DateTime.Now,
            CurrencyRefId = _selectedCurrencyId ?? (int)CurrenciesType.Khr
        };
        JournalLines.Clear();
        InitializeLines();
        ClearAlert();
        _visibled = false;
        StateHasChanged();
    }

    protected void CloseSelectedTab()
    {
        DialogService.Close(false);
    }

    // Alert helpers
    private void ShowAlert(string title, string message)
    {
        _isMessage = true;
        _titleMessage = $"{title}: ";
        _messageAlert = message;
    }

    private void ClearAlert()
    {
        _isMessage = false;
        _titleMessage = "";
        _messageAlert = "";
    }

    [Parameter] public EventCallback<int> Close { get; set; }
}