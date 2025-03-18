using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.Journals;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Journals
{
    public partial class JournalDetails
    {

        private readonly IJournalService jService;

        public JournalDetails(IJournalService journalService)
        {
            jService = journalService;
        }

        [Parameter] public long JournalId { get; set; }
    
        [Parameter] public JournalEntryDto? JournalEntry { get; set; } = new();
        [Parameter]
        public EventCallback<int> SelectIndex { get; set; }

        void RowClickEventArg(RowRenderEventArgs<JournalLineDto> e)
        {
            var rowIndex = JournalEntry!.JournalEntryLines!.IndexOf(e.Data);
            if (rowIndex % 2 == 0)
            {
                e.Attributes.Add("style", "background-color:var(--rz-base-100);height:32px;cursor:pointer;");
            }
            else
            {
                e.Attributes.Add("style", "background-color:var(--rz-base-200);height:32px;cursor:pointer;");
            }
        }

        void RowSelecting()
        {

        }

        protected async void SelectedIndex()
        {
            await SelectIndex.InvokeAsync(0);
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }


    }
}