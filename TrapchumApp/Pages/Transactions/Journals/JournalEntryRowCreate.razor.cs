using Applications.Requests.Transactions.Journals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Services.Lists;
using Applications.Services.Transactions.Journals;
using Microsoft.JSInterop;

namespace TrapchumApp.Pages.Transactions.Journals;

public partial class JournalEntryRowCreate(ILedgerServiceImpl ledgerService, IJournalServiceImpl journalService, IOtherNameServiceImpl nameService)
{
    [Parameter] public JournalLineDtoCreate JournalLine { get; set; } = new();
    [Parameter] public IEnumerable<VNameList> NameData { get; set; } = [];
    [Parameter] public IEnumerable<JournalLineDtoCreate> JournalLineList { get; set; } = [];
    [Parameter] public IEnumerable<VLedger>? LedgerData { get; set; } = [];
    [Parameter] public EventCallback AddRow { get; set; }
    [Parameter] public EventCallback<JournalLineDtoCreate> RemovedRow { get; set; }
    [Parameter] public EventCallback<JournalLineDtoCreate> DebitValidated { get; set; }
    [Parameter] public EventCallback<JournalLineDtoCreate> CreditValidated { get; set; }
    [Parameter] public EventCallback<JournalLineDtoCreate> ReCalculateTotal { get; set; }
    [Parameter] public int JournalLineSeq { get; set; } = 1;
    [Parameter] public EventCallback TotalValue { get; set; }
    [Parameter] public int RowIndex { get; set; } = 0;
    [Parameter] public string? Id { get; set; } = Guid.NewGuid().ToString("N");
    protected async void AddNewRow()
    {
        await AddRow.InvokeAsync();
    }

    protected async void RemoveRow()
    {
        await RemovedRow.InvokeAsync(JournalLine);
    }

    protected async void GetTotalValue()
    {
        await TotalValue.InvokeAsync();
    }

    async Task LoanNameData(LoadDataArgs args)
    {
        var filter = args.Filter ?? string.Empty;
        await GetNameLists(null, filter);
    }

    protected void GetCreditAmount(JournalLineDtoCreate journalLine)
    {
        if (journalLine.Credit > 0)
        {
            var credit = JournalLineList.LastOrDefault();

            if (credit!.Credit > 0)
            {
                AddNewRow();
                StateHasChanged();
            }
        }
    }

    protected void GetDebitAmount(JournalLineDtoCreate journalLine)
    {
        if (journalLine.Debit > 0)
        {
            var debit = JournalLineList.LastOrDefault();

            if (debit!.Debit > 0)
            {
                AddNewRow();
                StateHasChanged();
            }
        }
    }

    decimal? _savedValue = 0;

    protected override async Task OnInitializedAsync()
    {
        await GetNameLists(null);
        await base.OnInitializedAsync();
    }


    protected async void DebitValidate(JournalLineDtoCreate journalLine)
    {
        GetDebitAmount(journalLine);

        await DebitValidated.InvokeAsync(journalLine);
    }

    protected async void CreditValidate(JournalLineDtoCreate journalLine)
    {
        GetCreditAmount(journalLine);

        await CreditValidated.InvokeAsync(journalLine);
    }

    int _accApCount = 0;
    bool _isFocusDebit = false;
    bool _isFocusCredit = false;

    protected void OnNextPerform(JournalLineDtoCreate journalLine)
    {
        js.InvokeVoidAsync("", "");
    }

    void OnNedLine(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Key == "Enter" || eventArgs.Key == "NumpadEnter")
        {
            if (JournalLine!.LedgerRefId > 0)
            {
                AddNewRow();
            }
        }
    }

    void OnNedLine()
    {
        if (JournalLine!.LedgerRefId > 0)
        {
            AddNewRow();
        }
    }


    private void SelectedName(object? arg)
    {
        var name = ((VNameList)arg!)?.FullName;

        if (name != null)
        {
            JournalLine.EntityName = name;
        }
    }

    private async void SelectedItemChange()
    {
        var ledger = LedgerData!.Where(x => x.LedgerId == JournalLine.LedgerRefId).FirstOrDefault();

        if (ledger != null)
        {
            JournalLine.LedgerName = ledger.LedgerName!;
        }

        if (RowIndex == 1)
        {
        }
        else if (RowIndex > 1)
        {
            await ReCalculateTotal.InvokeAsync(JournalLine);
        }
    }

    protected void OnGetSavedValue(JournalLineDtoCreate journalLine)
    {
        GetTotalValue();
    }

    protected void OnBalanceTotal(JournalLineDtoCreate journalLine)
    {
        if (journalLine.Credit > 0 || journalLine.Debit > 0)
        {
            if (JournalLineList.Count() == 1)
            {
                AddNewRow();
            }
        }
    }

    VNameList? _nameList;
    bool isDataLoaded = false;
    int pageIndex = 0;
    int pageSize = 10;
    int counts = 0;
    string sortColumn = "FullName";

    async Task LoanNameList(LoadDataArgs args)
    {

        pageIndex = args.Skip ?? 0;

        pageSize = args.Top ?? 10;

        var filter = args.Filter;

        await GetNameLists(null, filter);
    }

    private async Task GetNameLists(string? id, string? searchText = null)
    {
        Tracker.Show();

        var param = new QueryParam(pageIndex, pageSize, sortColumn, true, false, searchText);

        var pager = await nameService.GetNameAsync<VNameList>(id, param);

        NameLists = pager.Items;

        counts = pager.Count;

        Tracker.Hide();

        StateHasChanged();
    }

    IEnumerable<VNameList> NameLists = [];
    protected async Task<bool> ValidateNameType(JournalLineDtoCreate journalLine)
    {
        try
        {
            if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
            {
                return false;
            }

            var ledgerResp = await ledgerService!.FindLedgerByIdAsync(journalLine.LedgerRefId);

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

            if (ledgerResp.IsSuccess)
            {
                var ledger = ledgerResp.Data;

                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS PAYABLE")
                {
                    _accApCount++;
                    //check vendor
                    if (_nameList?.EntType?.ToUpper() != "VENDOR" || journalLine.EntityNameRefId == null)
                    {
                        return false;
                    }
                }

                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS RECEIVABLE")
                {
                    //check customer
                    _accApCount++;
                    if (_nameList?.EntType?.ToUpper() != "CUSTOMER" || journalLine.EntityNameRefId == null)
                    {
                        return false;
                    }
                }
            }

            if (_accApCount > 1)
            {
                //You cannot use more than A/R or A/P account in the same transaction.
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
}