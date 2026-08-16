using Applications.Extensions.mappers;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Journals.Command.CreateJournal;
using Applications.Features.Journals.Dtos;
using Applications.Features.Journals.Queries.GetJournalEntryDetailById;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgerByCurrency;
using Applications.Features.OtherNames.Queries.GetOtherNames;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Transactions.Journals;

namespace TrapchumApp.Pages.Transactions.Journals;

public partial class JournalEntryUpdatePage(IJournalServiceImpl journalService)
{

    [Parameter] public string? JournalId { get; set; }
    [Parameter] public int BranchId { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;

    protected JournalEntryDto JLe = new();
    protected List<JournalEntryLineDto> JournalLines = [];
    private IEnumerable<CurrencyDto>? _currencies = [];
    private IEnumerable<JournalTypeDto>? _journalTypes = [];
    protected IEnumerable<VLedger>? Ledgers = [];
    protected IEnumerable<VOtherNames> NameLists = [];
    private bool _isMessage;
    private string _messageAlert = "";
    private string _titleMessage = "";
    private int? _selectedCurrencyId;
    private bool _visibled;
    private bool _isLoading = true;
    private string _filter = "";
    protected decimal TotalDebit => JournalLines.Sum(x => x.Debit ?? 0);
    protected decimal TotalCredit => JournalLines.Sum(x => x.Credit ?? 0);
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

        var journalTypes = await journalService.GetJournalTypesAsync(null);

        if (journalTypes.Any())
        {
            _journalTypes = journalTypes.Maps<JournalTypeDto>();
        }
    }

    private async Task LoadJournalEntry()
    {
        var resp = await mediator.Send(new GetJournalEntryDetailByIdQuery(JournalId, true));

        if (resp.IsError)
        {
            _toastService.ShowError(resp.GetMessage(), "Journal");

            return;
        }

        JLe = resp.Value;

        JournalLines = JLe.JournalEntryLines.ToList();

        if (JournalLines.Count == 0)
        {
            JournalLines.Add(new JournalEntryLineDto { LineSeq = 1 });
        }

        _selectedCurrencyId = JLe.CurrencyRefId;
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
        var seq = JournalLines.Count + 1;

        JournalLines.Add(new JournalEntryLineDto { LineSeq = seq });
    }

    protected void DeleteLine(JournalEntryLineDto line)
    {
        JournalLines.Remove(line);

        ResetLineSequence();

        if (JournalLines.Count == 0)
        {
            JournalLines.Add(new JournalEntryLineDto { LineSeq = 1 });
        }
    }

    private void ResetLineSequence()
    {
        for (int i = 0; i < JournalLines.Count; i++)
        {
            JournalLines[i].LineSeq = i + 1;
        }
    }

    private void AutoAddRowIfLast(JournalEntryLineDto row)
    {
        if (row == JournalLines.Last())
        {
            AddLine();
        }
    }

    // Selection handlers
    private void OnAccountSelected(object? obj, JournalEntryLineDto row)
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

    private void OnNameSelected(object? arg, JournalEntryLineDto row)
    {
        if (arg is VOtherNames name)
        {
            row.EntityName = name.AltName;

            row.EntityNameRefId = name.Id;
        }
    }

    private void OnAmountChanged(JournalEntryLineDto row)
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

    private void AutoBalanceLine(JournalEntryLineDto row)
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
        }

        StateHasChanged();
    }

    // Date validation
    private void OnValidateDate()
    {
        if (JLe.JournalDate is null)
        {
            return;
        }

        var day = (DateTime.Now - JLe.JournalDate.Value).Days;

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

        if (!JournalLines.Any())
        {
            _visibled = false;

            _messageAlert = "No journal lines to save.";

            _toastService.ShowWarning(_messageAlert, "Journal");

            return;
        }

        try
        {
            var validLines = JournalLines
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

            var entity = new JournalEntryDtoCreate
            {
                JournalEntryId = JLe.JournalEntryId,
                TransactionsRefId = JLe.TransactionsId,
                BranchRefId = JLe.BranchRefId,
                BankRefId = JLe.BankRefId,
                JournalType = JLe.JournalType,
                JournalDate = JLe.JournalDate,
                IsAdjustment = JLe.IsAdjustment,
                ReferrenceNum = JLe.ReferrenceNum,
                CurrencyRefId = JLe.CurrencyRefId,
                ExchangeRate = JLe.ExchangeRate,
                TotalDr = TotalDebit,
                TotalCr = TotalCredit,
                //JournalLineCount = validLines.Count,
                JournalDesc = JLe.JournalDesc,
                JournalEntryLines = validLines.Select((line, index) => new JournalLineDtoCreate
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

            var tranx = new JournalTransactionDtoCreate
            {
                TransactionsId = $"JNL_{Guid.CreateVersion7()}",
                CurrencyRefId = JLe.CurrencyRefId,
                BranchRefId = JLe.BranchRefId,
                UserRefId = userId,
                TranTypeRefId = (int)TransactionTypes.JORNL,
                TransactionDate = DateTimeOffset.UtcNow,
                JournalEntry = entity
            };

            var resp = await mediator.Send(new CreateJournalCommand(tranx));

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
