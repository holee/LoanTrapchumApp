using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Journals
{
    public partial class JournalEntryCreate
    {
        int? valueSize = 10;
        bool visibled = false;
        [Parameter]
        public EventCallback<int> Close { get; set; }

        protected async void CloseSelectedTab()
        {
            await Close.InvokeAsync(0);
            StateHasChanged();
        }
        private async void SelectedCurrencyChanged()
        {
            await GetLedgerByCurrency(selectedCurrencyId, null);
            jLe.JournalEntryLines!.Clear();
            AddNewRow();
            // await OnChangedByCurrency.InvokeAsync(selectedCurrencyId!.Value);
            await InvokeAsync(StateHasChanged);
        }
    }
}