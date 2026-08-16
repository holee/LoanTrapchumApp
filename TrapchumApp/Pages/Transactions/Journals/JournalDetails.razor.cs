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

        async Task ShowEditTab()
        {
            if (JournalEntry is null)
            {
                return;
            }

            var options = new DialogOptions
            {
                Width = "100vw",
                Height = "100vh",
                Draggable = true,
                Resizable = true,
                CloseDialogOnEsc = true,
                ShowClose = true
            };

            var parameters = new Dictionary<string, object?>
            {
                { "JournalId", JournalEntry.JournalEntryId },
                { "BranchId", JournalEntry.BranchId ?? 0 }
            };

            var dia = await DialogService.OpenAsync<JournalEntryUpdatePage>("Journal Entry", parameters, options);

            if (dia is bool success && success)
            {
                await GetJournalEntry(JournalId);

                StateHasChanged();
            }
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