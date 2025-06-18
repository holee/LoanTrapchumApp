using Applications.Extensions.mappers;
using Applications.Requests.Journals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Applications.Services.Transactions.Journals;
using Radzen.Blazor;
using Domains.Models.Identity;

namespace TrapchumApp.Pages.Transactions.Journals
{
    public partial class JournalIndex
    {
        [Parameter] public int BranchId { get; set; } = 0;
        private readonly IJournalServiceImpl _journalService;
        [Parameter] public int selectedIndex { get; set; } = 0;
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

        public JournalIndex(IJournalServiceImpl journalService)
        {
            _journalService = journalService;
        }

        string SearchText = string.Empty;
        string types = string.Empty;
        int pageIndex = 0;
        int pageSize = 15;
        string sortColumn = "TransactionDate";
        bool sortDescending = false;
        int counts = 0;
        bool isDataLoading = false;
        bool isLoading = true;
        bool showCreateTab = false;
        bool showDetailTab = false;
        bool IsNewEdit = false;
        bool IsEnableDetail = false;
        List<string> indexCounts = [];
        RadzenTabs tb = default!;
        IEnumerable<JournalTypeDto>? JournalTypes { get; set; }
        List<JournalEntryVm>? Journals { get; set; } = [];
        List<int> SelectedRow = [10, 20, 50, 100];
        IEnumerable<JournalEntryVm> JournalItems = [];
        JournalEntryVm? modelDetails = new();
        private string? _selectedJournalId;
        string? UserId = string.Empty;
        async Task ShowCreatedTab()
        {
            //if (!showCreateTab)
            //{
            //    showCreateTab = true;
            //    selectedIndex = 1;
            //    return;
            //}

            //selectedIndex = 0;

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

            var dia = await DialogService.OpenAsync<JournalEntryCreate>("Jorunal Entry", parameters, options);

            if (dia is bool succes && succes)
            {
                await GetJournals(SearchText);
            }

        }

        void CloseCreateTab()
        {
            showCreateTab = !showCreateTab;
            indexCounts.Remove("create");
            selectedIndex = 0;
        }

        void CloseDetailTab()
        {
            showDetailTab = !showDetailTab;
            indexCounts.Remove("detail");
            selectedIndex = 0;
        }

        void RowSelecting()
        {
        }

        async void ChangedPageSize(int page)
        {
            pageSize = page;
            Console.WriteLine(page);
            await GetJournals();
        }

        protected override async Task OnInitializedAsync()
        {

            var authState = await AuthState;

            UserId = authState.User.GetId();


            await GetJournals();

            var resp = await _journalService.GetJournalTypesAsync(null);

            if (resp.Any())
            {
                JournalTypes = resp.Maps<JournalTypeDto>();
            }

            await base.OnInitializedAsync();
        }

        private async Task GetJournals(string? filter = null)
        {
            try
            {
                Tracker.Show();

                var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filter);

                var journalItems = await _journalService.GetJournalsAsync(BranchId, param);

                counts = journalItems.Count;

                JournalItems = journalItems.Items;

                Tracker.Hide();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);

                Tracker.Hide();
            }
            finally
            {
                Tracker.Hide();
            }

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

        private async Task OnFilterType()
        {
            if (types == "")
                await GetJournals();
            else
                await GetJournals(types);
            await InvokeAsync(StateHasChanged);
        }

        void CloseDetailJournal(int e)
        {
            IsEnableDetail = false;
            selectedIndex = e;
            StateHasChanged();
        }

        void TabChanged(int e)
        {
            Console.WriteLine(e);
        }

        async void GetDetailJournal(string? journalId)
        {
            ContextMenuService.Close();

            _selectedJournalId = journalId;

            showDetailTab = true;

            selectedIndex = 2;

            StateHasChanged();
        }
    }
}