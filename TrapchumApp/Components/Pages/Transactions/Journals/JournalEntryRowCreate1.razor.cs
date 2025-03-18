using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Journals
{
    public partial class JournalEntryRowCreate
    {
        void ShowAdd(ElementReference elementReference, TooltipOptions? options = null) => tooltipService!.Open(elementReference, "Adding new row", options);
        void ShowDelete(ElementReference elementReference, TooltipOptions? options = null) => tooltipService!.Open(elementReference, "Romove row", options);
        void GetName()
        {
            var name = NameData.FirstOrDefault(x => x.Id == JournalLine.EntityNameRefId);
            if (name != null)
            {
                JournalLine.EntityName = name.FullName;
            }
        }
        private async void SelectedItemChange()
        {
            var ledger = LedgerData!.Where(x => x.LedgerId == JournalLine.LedgerRefId).FirstOrDefault();
            if (ledger != null)
            {
                JournalLine.LedgerName = ledger.LedgerName;
            }
            if (RowIndex == 1)
            {


            }
            else if (RowIndex > 1)
            {
                await ReCalculateTotal.InvokeAsync(JournalLine);
            }

        }
    }
}