using Radzen.Blazor;
using Domains.Models.Identity;
using Applications.Features.Journals.Queries.GetJournalAsPaging;
using Applications.Features.Journals.Queries.GetJournalTypes;
using Applications.Features.Journals.Dtos;

namespace TrapchumApp.Pages.Transactions.Journals
{
    public partial class JournalIndexPage
    {
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public int selectedIndex { get; set; } = 0;
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

        string SearchText = string.Empty;
        int pageIndex = 0;
        int pageSize = 15;
        string sortColumn = nameof(JournalEntryVm.TransactionDate);
        bool sortDescending = true;
        int counts = 0;
        bool showCreateTab = false;
        List<string> indexCounts = [];
        RadzenTabs tb = default!;
        IEnumerable<JournalTypeDto>? JournalTypes = [];
        IEnumerable<JournalEntryVm> JournalItems = [];
        string? UserId = string.Empty;

        async Task ShowCreatedTab()
        {
            var options = new DialogOptions
            {
                Width = "100vw",
                Height = "100vh"
            };

            var parameters = new Dictionary<string, object?>
            {
                {"BranchId",BranchId }
            };

            var dia = await DialogService.OpenAsync<JournalEntryCreate>("Jorunal Entry", parameters, options);

            if (dia is bool succes && succes)
            {
                await GetJournals(SearchText);
            }

        }


        async Task ShowEditTab()
        {
            var options = new DialogOptions
            {
                Width = "100vw",
                Height = "100vh"
            };

            var parameters = new Dictionary<string, object?>
            {
                {"BranchId",BranchId },
                {"UserId",UserId }
            };

            var dia = await DialogService.OpenAsync<JournalEntryUpdatePage>("Jorunal Entry", parameters, options);

            if (dia is bool succes && succes)
            {
                await GetJournals(SearchText);
            }

        }

        void RowSelecting()
        {
        }

        protected override async Task OnInitializedAsync()
        {

            var authState = await AuthState;

            UserId = authState.User.GetId();


            await GetJournals();

            var resp = await mediator.Send(new GetJournalTypesQuery(0, new QueryParam(0, 0)));

            JournalTypes = resp.Items;

            await base.OnInitializedAsync();
        }

        private async Task GetJournals(string? filter = null)
        {
            Tracker.Show();

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filter);

            var journalItems = await mediator.Send(new GetJournalAsPagingQuery(BranchId, param));

            counts = journalItems.TotalCount;

            JournalItems = journalItems.Items;

            Tracker.Hide();

            StateHasChanged();
        }

        async Task LoadData(LoadDataArgs e)
        {
            pageIndex = e.Skip ?? 0;

            var filter = e.Filter ?? string.Empty;

            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortColumn = e.OrderBy.Split(" ")[0];

                var dir = e.OrderBy.Split(" ")[1];

                sortDescending = !dir.Contains("asc");
            }

            await GetJournals(filter);
        }

        private async Task PerformSearch(string? e)
        {
            SearchText = e ?? string.Empty;

            await GetJournals(SearchText);

            await InvokeAsync(StateHasChanged);
        }


        void TabChanged(int e)
        {
            Console.WriteLine(e);
        }

    }
}