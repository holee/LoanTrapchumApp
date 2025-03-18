using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.Journals;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Responses.ViewModels.OtherNames;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TrapchumApp.Components.Pages.Transactions.Journals;

public partial class JournalEntryRowCreate
{

    private readonly ILedgerService _LedgerService;
    private readonly IJournalService _JournalService;
    public JournalEntryRowCreate(ILedgerService ledgerService, IJournalService journalService)
    {
        _LedgerService = ledgerService;
        _JournalService = journalService;
    }

    [Parameter, EditorRequired] public JournalLineDtoCreate JournalLine { get; set; } = new();
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
    protected IEnumerable<EntityNameVm>? EntityNames { get; set; } = [];
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
    decimal? SavedValue = 0;
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
    int AccAPCount = 0;
    bool isFocusDebit = false;
    bool isFocusCredit = false;
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
    VNameList? nameList;

    protected async Task<bool> ValidateNameType(JournalLineDtoCreate journalLine)
    {
        try
        {
            if (journalLine.LedgerRefId == null || journalLine.LedgerRefId == 0)
            {
                return false;
            }
            var ledgerResp = await _LedgerService!.FindLedgerByIdAsync(journalLine.LedgerRefId);
            var names = await _JournalService!.FindNameLists(journalLine.EntityNameRefId);
            if (names.Any())
            {
                nameList = names.FirstOrDefault();
                journalLine.EntityName = nameList?.FullName;
            }
            else
            {
                nameList = null;
            }

            if (ledgerResp.IsSuccess)
            {

                var ledger = ledgerResp.Data;

                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS PAYABLE")
                {
                    AccAPCount++;
                    //check vendor
                    if (nameList?.EntType?.ToUpper() != "VENDOR" || journalLine.EntityNameRefId == null)
                    {
                        return false;
                    }

                }
                if (ledger?.LedgerType?.LedgerTypeCode?.ToUpper() == "ACCOUNTS RECEIVABLE")
                {
                    //check customer
                    AccAPCount++;
                    if (nameList?.EntType?.ToUpper() != "CUSTOMER" || journalLine.EntityNameRefId == null)
                    {
                        return false;
                    }
                }
            }
            if (AccAPCount > 1)
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
