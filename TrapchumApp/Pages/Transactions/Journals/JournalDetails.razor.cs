using Applications.Features.Transactions.Journals;
using Applications.Responses.ViewModels.Journals;
using Applications.Services.Transactions.Journals;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Journals
{
    public partial class JournalDetails(IJournalServiceImpl journalService)
    {
        [Parameter] public string? JournalId { get; set; }
        [Parameter] public JournalEntryVm? JournalEntry { get; set; } = new();
        [Parameter] public EventCallback<int> SelectIndex { get; set; } 
        [Parameter] public EventCallback<int> CloseTab { get; set; }
        private IEnumerable<JournalEntryLineVm> _journalEntryLines = [];
        void RowClickEventArg(RowRenderEventArgs<JournalLineDto> e)
        {
            // var rowIndex = JournalEntry!.JournalEntryLines!.IndexOf(e.Data);
            //
            // if (rowIndex % 2 == 0)
            // {
            //     e.Attributes.Add("style", "background-color:var(--rz-base-100);height:32px;cursor:pointer;");
            // }
            // else
            // {
            //     e.Attributes.Add("style", "background-color:var(--rz-base-200);height:32px;cursor:pointer;");
            // }
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
            await GetJournalEntry(JournalId);
            await base.OnInitializedAsync();
        }
        
        async Task ClosedTabEvent()
        {
            await CloseTab.InvokeAsync(2);
        }

        async Task GetJournalEntry(string? journalId)
        {
            if (!string.IsNullOrEmpty(journalId) || JournalEntry is null)
            {
                var resp = await journalService.GetJournalByIdAsync(journalId, true);
                JournalEntry = resp.IsError ? null! : resp.Value;
                _journalEntryLines = resp.IsError ? [] : resp.Value?.JournalEntryLines ?? [];
            }
        }
    }
}