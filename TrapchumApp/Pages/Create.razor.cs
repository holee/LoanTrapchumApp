using Applications.Constants;
using Applications.Extensions.mappers;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Journals.Command.CreateJournal;
using Applications.Features.Journals.Dtos;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgerByCurrency;
using Applications.Features.OtherNames.Queries.GetOtherNames;
using Applications.Helpers;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Transactions.Journals;

namespace TrapchumApp.Pages;

public partial class Create
{
    [Inject] private ILedgerServiceImpl ledgerService { get; set; } = default!;
    [Inject] private IJournalServiceImpl journalService { get; set; } = default!;

    protected JournalEntryDtoCreate JLe = new();
    protected List<JournalLineDtoCreate> JournalLines = [];
    protected IEnumerable<CurrencyDto>? _currencies = [];
    protected IEnumerable<JournalTypeDto>? _journalTypes = [];
    protected IEnumerable<VLedger>? Ledgers = [];
    protected IEnumerable<VOtherNames> NameLists = [];
    protected bool _isMessage;
    protected string _messageAlert = "";
    protected string _titleMessage = "";
    protected int? _selectedCurrencyId;
    protected bool _visibled;
    protected string _filter = "";

    [Parameter] public string? UserId { get; set; }
    [Parameter] public int BranchId { get; set; }

    private const int MinimumVisibleRows = 8;

    protected int VisibleRowCount => Math.Max(MinimumVisibleRows, JournalLines.Count);
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
            JLe.CurrencyRefId = (int)CurrenciesType.KHR;

            InitializeLines();
            await LoadRelatedData();
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
        JournalLines.Clear();
        for (int i = 1; i <= MinimumVisibleRows; i++)
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
            var res = await mediator.Send(new GetLedgerByCurrencyQuery(0, currencyId, new QueryParam(0, 0, string.Empty, false, false, searchText)));
            Ledgers = res.Items;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message, "Error");
        }
    }

    protected async Task LoadDataLedger(LoadDataArgs arg)
    {
        if (arg is not null && !string.IsNullOrEmpty(arg.Filter))
        {
            _filter = arg.Filter;
        }
        await GetLedgerByCurrency(_selectedCurrencyId, _filter);
    }

    protected async Task LoadNamesList(LoadDataArgs arg)
    {
        await GetNameLists(null, arg.Filter ?? string.Empty);
    }

    private string[] columns = { nameof(VOtherNames.Id), nameof(VOtherNames.EntType), nameof(VOtherNames.AltName) };

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

    protected void AddLine()
    {
        var seq = JournalLines.Count + 1;
        JournalLines.Add(new JournalLineDtoCreate { LineSeq = seq });
    }

    protected void ClearLines()
    {
        JournalLines.Clear();
        InitializeLines();
        StateHasChanged();
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

    protected void OnAccountSelected(object? obj, JournalLineDtoCreate row)
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

    protected void OnNameSelected(object? arg, JournalLineDtoCreate row)
    {
        if (arg is VOtherNames name)
        {
            row.EntityName = name.AltName;
        }
    }

    protected void OnAmountChanged(JournalLineDtoCreate row)
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

    protected async Task SelectedCurrencyChanged(object? obj)
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

    protected void OnValidateDate()
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

    protected async Task HandleFormSave()
    {
        _visibled = true;

        if (!JournalLines.Any())
        {
            _visibled = false;
            ShowAlert("No Journal Lines", "No journal lines to save.");
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
            //JLe.JournalLineCount = validLines.Count;
            //JLe.JournalEntryLines = validLines.Select((line, index) => new JournalEntryLineDtoCreate
            //{

            //});
            trx.JournalEntry = JLe;

            var command = new CreateJournalCommand(trx);
            var resp = await mediator.Send(command);

            if (resp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "Journal", "Journal Entry created successfully!");
                ClearForm();
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

            VOtherNames? nameItem = null;
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

            await InvokeAsync(StateHasChanged);
            return (true, accApCount);
        }
        catch
        {
            return (true, accApCount);
        }
    }

    protected void ClearForm()
    {
        JLe = new JournalEntryDtoCreate
        {
            JournalType = JournalTypes.PV.ToString(),
            JournalDate = DateTime.Now,
            CurrencyRefId = _selectedCurrencyId ?? (int)CurrenciesType.KHR
        };
        JournalLines.Clear();
        InitializeLines();
        ClearAlert();
        _visibled = false;
        StateHasChanged();
    }

    protected void Cancel()
    {
        // Add cancel routing or navigation
    }

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
}
