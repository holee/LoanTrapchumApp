using Applications.Extensions.mappers;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Journals.Command.UpdateJournal;
using Applications.Features.Journals.Dtos;
using Applications.Features.Journals.Queries.GetJournalEntryForUpdate;
using Applications.Features.Journals.Queries.GetJournalTypes;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgerByCurrency;
using Applications.Features.OtherNames.Queries.GetOtherNames;
using Applications.Responses.ViewModels.OtherNames;
using Radzen.Blazor;

namespace Web.UI.Pages.Transactions.Journals;

public partial class JournalEntryUpdatePage
{

    [Parameter] public string? JournalId { get; set; }
    [Parameter] public string? TransactionId { get; set; }
    [Parameter] public int BranchId { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;

    protected JournalEntryDtoUpdate _entry = new();
    private JournalTransactionDtoUpdate _tranx = new();
    protected List<JournalLineDtoUpdate> _entryLines = new();
    private IEnumerable<CurrencyDto>? _currencies = new List<CurrencyDto>();
    private IEnumerable<JournalTypeDto>? _journalTypes = new List<JournalTypeDto>();
    protected IEnumerable<VLedger>? Ledgers = new List<VLedger>();
    protected IEnumerable<VOtherNames> NameLists = new List<VOtherNames>();
    private bool _isMessage;
    private string _messageAlert = "";
    private string _titleMessage = "";
    private int? _selectedCurrencyId;
    private bool _visibled;
    private bool _isLoading = true;
    private string _filter = "";

    private RadzenDataGrid<JournalLineDtoUpdate> _journalGrid = default!;
    private string _columnEditing = string.Empty;
    private readonly List<JournalLineDtoUpdate> _rowsToUpdate = [];
    protected bool IsFillerLine(JournalLineDtoUpdate jl) => !_entryLines.Contains(jl);

    private async Task OnCellClick(DataGridCellMouseEventArgs<JournalLineDtoUpdate> args)
    {
        if (args.Data is null) return;
        if (args.Column is null) return;
        if (string.IsNullOrEmpty(args.Column.Property)) return;
        if (IsFillerLine(args.Data)) return;

        if (_rowsToUpdate.Contains(args.Data) && _columnEditing == args.Column.Property) return;

        if (_rowsToUpdate.Any())
        {
            await _journalGrid.UpdateRow(_rowsToUpdate.First());
        }

        _columnEditing = args.Column.Property;
        await EditRow(args.Data);
    }

    private async Task EditRow(JournalLineDtoUpdate line)
    {
        if (!_rowsToUpdate.Contains(line))
        {
            _rowsToUpdate.Add(line);
        }
        await _journalGrid.EditRow(line);
    }

    private bool IsEditing(string columnName, JournalLineDtoUpdate line)
    {
        return _columnEditing == columnName && _rowsToUpdate.Contains(line);
    }

    private void OnRowUpdate(JournalLineDtoUpdate line)
    {
        _rowsToUpdate.Remove(line);
        _columnEditing = string.Empty;
    }

    protected decimal TotalDebit => _entryLines.Sum(x => x.Debit ?? 0);
    protected decimal TotalCredit => _entryLines.Sum(x => x.Credit ?? 0);
    protected decimal Difference => Math.Abs(TotalDebit - TotalCredit);
    protected bool IsBalanced => TotalDebit == TotalCredit && TotalDebit > 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _isLoading = true;

            await LoadRelatedData();

            await LoadJournalEntry();

            await GetLedgerByCurrency(_selectedCurrencyId);

            await GetNameLists(null);

            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Journal");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task LoadRelatedData()
    {
        var command = new GetCurrenciesQuery(0, new QueryParam(0, 0));

        var rsp = await mediator.Send(command);

        _currencies = rsp.Items.Maps<CurrencyDto>();

        var journalTypes = await mediator.Send(new GetJournalTypesQuery(0, new QueryParam(0, 0)));

        _journalTypes = journalTypes.Items;

    }

    private async Task LoadJournalEntry()
    {
        var resp = await mediator.Send(new GetJournalEntryForUpdateByIdQuery(TransactionId, BranchId));

        if (resp.IsError)
        {
            _toastService.ShowError(resp.GetMessage(), "Journal");

            return;
        }

        _tranx = resp.Value;

        _entry = _tranx.JournalEntry ?? new();

        _entryLines = _entry.JournalEntryLines ?? [];

        if (_entryLines.Count == 0)
        {
            _entryLines.Add(new JournalLineDtoUpdate { LineSeq = 1 });
        }

        _selectedCurrencyId = _entry.CurrencyRefId;
    }

    private async Task GetLedgerByCurrency(int? currencyId, string? searchText = null)
    {
        try
        {

            var param = new QueryParam(0, 0, string.Empty, false, false, searchText);

            var res = await mediator.Send(new GetLedgerByCurrencyQuery(0, currencyId, param));

            Ledgers = res.Items;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Ledger");
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

            string[] columns = {
                nameof(VOtherNames.Id),
                nameof(VOtherNames.AltName),
                nameof(VOtherNames.EntType),
            };

            var resp = await mediator.Send(new GetOtherNamesQuery(id, param, columns));

            NameLists = resp.Items;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Name Lists");
        }
    }

    // Row management
    protected void AddLine()
    {
        var seq = _entryLines.Count + 1;

        _entryLines.Add(new JournalLineDtoUpdate { LineSeq = seq });

        _ = _journalGrid.Reload();
    }

    protected void DeleteLine(JournalLineDtoUpdate line)
    {
        _entryLines.Remove(line);

        _rowsToUpdate.Remove(line);

        ResetLineSequence();

        if (_entryLines.Count == 0)
        {
            _entryLines.Add(new JournalLineDtoUpdate { LineSeq = 1 });
        }

        _ = _journalGrid.Reload();
    }

    private void ResetLineSequence()
    {
        for (int i = 0; i < _entryLines.Count; i++)
        {
            _entryLines[i].LineSeq = i + 1;
        }
    }

    private void AutoAddRowIfLast(JournalLineDtoUpdate row)
    {
        if (row == _entryLines.Last())
        {
            AddLine();
        }
    }

    // Selection handlers
    private void OnAccountSelected(object? obj, JournalLineDtoUpdate row)
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

    private void OnNameSelected(object? arg, JournalLineDtoUpdate row)
    {
        if (arg is VOtherNames name)
        {
            row.EntityName = name.AltName;

            row.EntityNameRefId = name.Id;
        }
    }

    private void OnAmountChanged(JournalLineDtoUpdate row)
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

    private void AutoBalanceLine(JournalLineDtoUpdate row)
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
        }

        StateHasChanged();
    }

    // Date validation
    private void OnValidateDate()
    {
        if (_entry.JournalDate is null)
        {
            return;
        }

        var day = (DateTime.Now - _entry.JournalDate.Value).Days;

        if (day > 30)
        {

            _toastService.ShowWarning("This transaction is more than 30 days in the past.", "Journal Date");

        }
        else if (day < -30)
        {
            _toastService.ShowWarning("This transaction is more than 30 days in the future.", "Journal Date");
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

            _toastService.ShowWarning(_messageAlert, "Journal");

            return;
        }

        try
        {
            var validLines = _entryLines
                .Where(x => x.LedgerRefId is not null and > 0 && (x.Credit > 0 || x.Debit > 0))
                .ToList();

            if (validLines.Count <= 1)
            {

                _toastService.ShowWarning("Cannot save this transaction with one journal line.", "Journal");

                _visibled = false;

                return;
            }

            if (!IsBalanced)
            {

                _toastService.ShowWarning("Total credit and debit is not balanced.", "Journal");

                _visibled = false;

                return;
            }

            var confirm = await MsgBox.ConfirmAsync("Are you sure you want to update this journal entry?", "Confirm Update");

            if (confirm == MsgBoxResult.No)
            {
                _visibled = false;

                return;
            }

            var entity = new JournalEntryDtoUpdate
            {
                JournalEntryId = _entry.JournalEntryId,
                TransactionsRefId = _entry.TransactionsRefId,
                BranchRefId = _entry.BranchRefId,
                BankRefId = _entry.BankRefId,
                JournalType = _entry.JournalType,
                JournalDate = _entry.JournalDate,
                IsAdjustment = _entry.IsAdjustment,
                ReferrenceNum = _entry.ReferrenceNum,
                CurrencyRefId = _entry.CurrencyRefId,
                ExchangeRate = _entry.ExchangeRate,
                JournalDesc = _entry.JournalDesc,
                JournalEntryLines = validLines.Select((line, index) => new JournalLineDtoUpdate
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
                }).ToList()
            };

            string? userId = (await AuthState).GetUserId();

            var tranx = new JournalTransactionDtoUpdate
            {
                TransactionsId = $"JNL_{Guid.CreateVersion7()}",
                CurrencyRefId = _entry.CurrencyRefId,
                BranchRefId = _entry.BranchRefId,
                UserRefId = userId,
                TranTypeRefId = (int)TransactionTypes.JORNL,
                TransactionDate = DateTimeOffset.UtcNow,
                JournalEntry = entity
            };

            var resp = await mediator.Send(new UpdateJournalCommand(tranx));

            if (resp.IsSuccess())
            {
                _toastService.ShowSuccess("Journal Entry updated successfully!", "Journal");

                DialogService.Close(true);
            }
            else
            {
                _toastService.ShowWarning(resp.GetMessage(), "Journal");

            }
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Journal");
        }
        finally
        {
            _visibled = false;

            ClearAlert();
        }
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
