using Applications.Features.Journals.Dtos;
using Applications.Features.Journals.Queries.GetJournalEntryDetailById;

namespace TrapchumApp.Pages.Transactions.Journals
{
    public partial class JournalDetails
    {
        [Parameter] public string? JournalId { get; set; }
        [Parameter] public JournalEntryDto? JournalEntry { get; set; } = new(); 
        [Parameter] public EventCallback<int> SelectIndex { get; set; }
        [Parameter] public EventCallback Close { get; set; }

        private IEnumerable<JournalEntryLineDto> _journalEntryLines = [];

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
            await Close.InvokeAsync();
        }

        async Task GetJournalEntry(string? journalId)
        {
            if (!string.IsNullOrEmpty(journalId) || JournalEntry is null)
            {
                var resp = await mediator.Send(new GetJournalEntryDetailByIdQuery(journalId, true));

                JournalEntry = resp.Value;

                _journalEntryLines = resp.Value.JournalEntryLines;
            }
        }
    }
}