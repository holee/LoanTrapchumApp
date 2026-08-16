using Applications.Commons.Validators;
using Applications.Constants;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Journals.Command.CreateJournal;
using Applications.Features.Journals.Dtos;
using Applications.Features.Journals.Queries.GetJournalTypes;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgerByCurrency;
using Applications.Features.Ledgers.Queries.GetLedgerDetailsById;
using Applications.Features.OtherNames.Queries.GetOtherNames;
using Applications.Helpers;
using Applications.Responses.ViewModels.OtherNames;
using Radzen.Blazor;
using System.Globalization;

namespace TrapchumApp.Pages.Transactions.Journals;

public partial class JournalEntryCreate
{
    [Parameter] public int BranchId { get; set; }
    protected JournalEntryDtoCreate _entry = new();
    protected List<JournalLineDtoCreate> _entryLines = [];
    protected JournalLineDtoCreate? _autoLine;
    private IEnumerable<CurrencyDto>? _currencies = [];
    private IEnumerable<JournalTypeDto>? _journalTypes = [];
    protected IEnumerable<VLedger>? Ledgers = [];
    protected IEnumerable<VOtherNames> NameLists = [];
    private bool _isMessage;
    private string _messageAlert = "";
    private string _statusMessage = "";
    private int? _selectedCurrencyId;
    private EditForm? _editForm;
    private bool _visibled;
    private string _filter = "";
    private string? _userId = string.Empty;
    private const int MinimumVisibleRows = 4;
    protected int VisibleRowCount => Math.Max(MinimumVisibleRows, _entryLines.Count);
    private ValidationResults? _validation;
    protected List<JournalLineDtoCreate> GridDisplayLines
    {
        get
        {
            var list = new List<JournalLineDtoCreate>(_entryLines);

            for (int i = _entryLines.Count + 1; i <= VisibleRowCount; i++)
            {
                list.Add(new JournalLineDtoCreate { LineSeq = i });
            }

            return list;
        }
    }

    private void OnDebitChanged(JournalEntryLineDtoCreate line, ChangeEventArgs e)
    {
        line.Debit = ParseAmount(e.Value?.ToString());

        if (line.Debit > 0)
        {
            line.Credit = 0;
        }

        OnAmountEdited(line);
    }

    private void OnCreditChanged(JournalEntryLineDtoCreate line, ChangeEventArgs e)
    {
        line.Credit = ParseAmount(e.Value?.ToString());

        if (line.Credit > 0)
        {
            line.Debit = 0;
        }

        OnAmountEdited(line);
    }

    private static decimal ParseAmount(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return 0m;
        }

        const NumberStyles styles = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;

        if (!decimal.TryParse(raw.Trim(), styles, CultureInfo.CurrentCulture, out var value) || value <= 0)
        {
            return 0m;
        }

        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }


    private void OnAmountEdited(JournalEntryLineDtoCreate? line)
    {
        if (ReferenceEquals(_autoLine, line))
        {
            _autoLine = null;
        }
        else
        {
            RefreshAutoAmount();
        }

        EnsureTrailingBlankLine();
    }

    private void RefreshAutoAmount()
    {
        if (_autoLine is null)
        {
            return;
        }

        // A line dropped from the grid stops being tracked.
        if (!_entry.JournalEntryLines.Contains(_autoLine))
        {
            _autoLine = null;

            return;
        }

        var difference = _entry.JournalEntryLines
            .Where(l => !ReferenceEquals(l, _autoLine))
            .Sum(l => l.Debit - l.Credit);

        _autoLine.Debit = difference < 0 ? -difference : 0;

        _autoLine.Credit = difference > 0 ? difference : 0;
    }

    private void EnsureTrailingBlankLine()
    {
        if (_entry.JournalEntryLines.Count == 0 || !_entry.JournalEntryLines[^1].IsEmpty)
        {
            AddLine();
        }
    }


    // A filler row is a placeholder used only to pad the grid's height; it is never part of JournalLines.
    protected bool IsFillerLine(JournalLineDtoCreate jl) => !_entryLines.Contains(jl);

    private RadzenDataGrid<JournalLineDtoCreate> _journalGrid = default!;
    private string _columnEditing = string.Empty;
    private readonly List<JournalLineDtoCreate> _rowsToUpdate = [];
    protected decimal TotalDebit => _entryLines.Sum(x => x.Debit ?? 0);
    protected decimal TotalCredit => _entryLines.Sum(x => x.Credit ?? 0);
    protected decimal Difference => Math.Abs(TotalDebit - TotalCredit);
    protected bool IsBalanced => TotalDebit == TotalCredit && TotalDebit > 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LoadRelatedData();

            _entry.JournalType = JournalTypes.PV.ToString();

            _entry.JournalDate = DateTime.Now;

            _entry.CurrencyRefId = (int)CurrenciesType.KHR;

            InitializeLines();

            await GetLedgerByCurrency((int)CurrenciesType.KHR);

            _selectedCurrencyId = (int)CurrenciesType.KHR;

            await GetNameLists(string.Empty);

            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Initialization Error");

        }
    }

    private void InitializeLines()
    {
        _entryLines.Clear();

        for (int i = 1; i <= MinimumVisibleRows; i++)
        {
            _entryLines.Add(new JournalLineDtoCreate { LineSeq = i });
        }

        _autoLine = null;

        _validation = null;

        _ = _journalGrid.Reload();
    }

    private async Task LoadRelatedData()
    {
        var command = new GetCurrenciesQuery(0, new QueryParam(0, 0));

        var rsp = await mediator.Send(command);

        _currencies = rsp.Items;

        var journalTypes = await mediator.Send(new GetJournalTypesQuery(0, new QueryParam(0, 0)));

        _journalTypes = journalTypes.Items;
    }

    private async Task GetLedgerByCurrency(int? currencyId, string? searchText = null)
    {
        try
        {
            var res = await mediator.Send(new GetLedgerByCurrencyQuery(0, currencyId, new QueryParam(0, 0, string.Empty, false, false, searchText)));

            Ledgers = res.Items;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Error");

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

    string[] columns = { nameof(VOtherNames.Id), nameof(VOtherNames.EntType), nameof(VOtherNames.AltName) };

    private async Task GetNameLists(string? id = null, string? searchText = null)
    {
        try
        {
            var param = new QueryParam(0, 0, string.Empty, false, false, searchText);

            var rsp = await mediator.Send(new GetOtherNamesQuery(id, param, columns));

            NameLists = rsp.Items;

            StateHasChanged();

        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Error");
        }
    }




    // In-cell edit handling
    private async Task OnCellClick(DataGridCellMouseEventArgs<JournalLineDtoCreate> args)
    {
        if (args.Data is null) return;
        if (args.Column is null) return;
        if (string.IsNullOrEmpty(args.Column.Property))
        {
            return;
        }

        if (IsFillerLine(args.Data))
        {
            return;
        }

        if (_rowsToUpdate.Contains(args.Data) && _columnEditing == args.Column.Property)
        {
            return;
        }

        if (_rowsToUpdate.Any())
        {
            await _journalGrid.UpdateRow(_rowsToUpdate.First());
        }

        _columnEditing = args.Column.Property;

        await EditRow(args.Data);
    }

    private async Task EditRow(JournalLineDtoCreate line)
    {
        if (!_rowsToUpdate.Contains(line))
        {
            _rowsToUpdate.Add(line);
        }

        await _journalGrid.EditRow(line);
    }

    private bool IsEditing(string columnName, JournalLineDtoCreate line)
    {
        return _columnEditing == columnName && _rowsToUpdate.Contains(line);
    }

    private void OnRowUpdate(JournalLineDtoCreate line)
    {
        _rowsToUpdate.Remove(line);

        _columnEditing = string.Empty;
    }

    // Row management
    protected void AddLine()
    {
        var seq = _entryLines.Count + 1;

        _entryLines.Add(new JournalLineDtoCreate { LineSeq = seq });

        _ = _journalGrid.Reload();
    }

    protected void DeleteLine(JournalLineDtoCreate line)
    {
        _entryLines.Add(line);

        _rowsToUpdate.Remove(line);

        //ResetLineSequence();

        //if (JournalLines.Count == 0)
        //{
        //    JournalLines.Add(new JournalLineDtoCreate { LineSeq = 1 });
        //}

        _ = _journalGrid.Reload();
    }

    private void ResetLineSequence()
    {
        for (int i = 0; i < _entryLines.Count; i++)
        {
            // JournalLines[i].LineSeq = i + 1;
        }
    }

    private void AutoAddRowIfLast(JournalLineDtoCreate row)
    {
        if (row == _entryLines.Last())
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

            var rowIndex = _entryLines.IndexOf(row);

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
        if (arg is VOtherNames name)
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

        var otherDebit = _entryLines.Where(x => x != row).Sum(x => x.Debit ?? 0);

        var otherCredit = _entryLines.Where(x => x != row).Sum(x => x.Credit ?? 0);

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

            _entryLines.Clear();
        }

        InitializeLines();

        StateHasChanged();
    }

    // Date validation
    private void OnValidateDate()
    {
        var day = (DateTime.Now - _entry.JournalDate!.Value).Days;

        if (day > 30)
        {
            _toastService.ShowWarning("This transaction is more than 30 days in the past.", "Past Transaction");

        }
        else if (day < -30)
        {
            _toastService.ShowWarning("This transaction is more than 30 days in the future.", "Future Transaction");
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

        if (!_entryLines.Any())
        {
            _visibled = false;

            _messageAlert = "No journal lines to save.";

            _toastService.ShowWarning(_messageAlert, "Warning");

            return;
        }

        try
        {
            var validLines = _entryLines
                .Where(x => x.LedgerRefId is not null and > 0 && (x.Credit > 0 || x.Debit > 0))
                .ToList();

            if (validLines.Count <= 1)
            {
                _toastService.ShowWarning("Cannot save this transaction with one journal line.", "Invalid JournalLine");

                _visibled = false;

                return;
            }

            //int accApCount = 0;

            //foreach (var line in validLines)
            //{
            //    var result = await ValidateNameType(line, accApCount);

            //    if (!result.IsValid)
            //    {
            //        _visibled = false;

            //        return;
            //    }

            //    accApCount = result.AccApCount;
            //}

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
                CurrencyRefId = _entry.CurrencyRefId,
                UserRefId = _userId,
                BranchRefId = BranchId
            };

            _entry.BranchRefId = BranchId;
            _entry.BankRefId = 11;
            _entry.TotalCr = TotalCredit;
            _entry.TotalDr = TotalDebit;
            //_entry.JournalLineCount = validLines.Count;
            _entry.JournalEntryLines = validLines.Select((line, index) => new JournalLineDtoCreate
            {
                JournalLineId = line.JournalLineId,
                JournalRefId = line.JournalRefId,
                LoanRefId = line.LoanRefId,
                EntityNameRefId = line.EntityNameRefId,
                EntityName = line.EntityName ?? string.Empty,
                LineSeq = index + 1,
                LedgerRefId = line.LedgerRefId,
                LedgerName = line.LedgerName,
                Amount = line.Amount,
                Debit = line.Debit,
                Credit = line.Credit,
                ExchangeRate = line.ExchangeRate,
                BaseAmount = line.BaseAmount,
                JournalLineDesc = line.JournalLineDesc
            }).ToList();

            trx.JournalEntry = _entry;

            var confirm = await MsgBox.ConfirmAsync("Are you sure you want to save this journal entry?", "Confirm Save");

            if (confirm == MsgBoxResult.No)
            {
                _visibled = false;

                return;
            }

            var command = new CreateJournalCommand(trx);

            var resp = await mediator.Send(command);

            if (resp.IsSuccess())
            {
                _toastService.ShowSuccess("Journal Entry created successfully!", "Success");

                await ClearForm();

                DialogService.Close(true);
            }
            else
            {
                _toastService.ShowWarning(resp.GetMessage(), "Warning");
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
                _toastService.ShowError("Some journal lines are missing an account.", "Missing Account");

                return (false, accApCount);
            }

            // await GetNameLists(journalLine.EntityNameRefId);

            VOtherNames? nameItem = NameLists.FirstOrDefault(x => x.Id == journalLine.EntityNameRefId);

            if (nameItem is not null)
            {
                //nameItem = entityName;
                journalLine.EntityName = nameItem?.FullName;
            }

            var resp = await mediator.Send(new GetLedgerDetailsByIdQuery(journalLine.LedgerRefId));

            if (resp.IsSuccess())
            {
                var ledger = resp.Value;

                var typeCode = ledger?.LedgerTypeName?.ToUpper();

                if (typeCode == LedgerTypeConstant.ACCOUNTS_PAYABLE)
                {
                    accApCount++;

                    if (nameItem?.EntType?.ToUpper() != EntityTypeConstant.VENDOR || journalLine.EntityNameRefId == null)
                    {
                        _toastService.ShowError("Please choose a vendor when using an Accounts Payable account.", "Required Name");

                        return (false, accApCount);
                    }
                }

                if (typeCode == LedgerTypeConstant.ACCOUNTS_RECEIVABLE)
                {
                    accApCount++;

                    if (nameItem?.EntType?.ToUpper() != EntityTypeConstant.CUSTOMER || journalLine.EntityNameRefId == null)
                    {

                        _toastService.ShowError("Please choose a customer when using an Accounts Receivable account.", "Required Name");

                        return (false, accApCount);
                    }
                }
            }

            if (accApCount > 1)
            {
                _toastService.ShowError("You cannot use more than one A/R or A/P account in the same transaction.", "Invalid Account");

                return (false, accApCount);
            }

            return (true, accApCount);
        }
        catch
        {
            return (false, accApCount);
        }
    }

    // Clear & Close
    protected async Task ClearForm()
    {
        _entry = new JournalEntryDtoCreate
        {
            JournalType = JournalTypes.PV.ToString(),
            JournalDate = DateTime.Now,
            CurrencyRefId = _selectedCurrencyId ?? (int)CurrenciesType.KHR
        };
        _entryLines.Clear();
        InitializeLines();
        ClearAlert();
        _visibled = false;

        // Reset the namelist dropdown back to the full, unfiltered list. Otherwise it stays
        // scoped to whatever search text/id was last used (e.g. via LoadNamesList/ValidateNameType)
        // and the grid's name dropdown looks stale on the next journal entry.
        await GetNameLists(string.Empty);

        // StateHasChanged();
    }

    protected void CloseSelectedTab()
    {
        DialogService.Close(false);
    }

    // Alert helpers
    private void ShowAlert(string title, string message)
    {
        _isMessage = true;
        _statusMessage = $"{title}: ";
        _messageAlert = message;
    }

    private void ClearAlert()
    {
        _isMessage = false;
        _statusMessage = "";
        _messageAlert = "";
    }

    [Parameter] public EventCallback<int> Close { get; set; }
}