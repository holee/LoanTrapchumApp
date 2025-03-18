using Applications.Extensions.mappers;
using Applications.Interfaces.Transactions;
using Applications.Requests.Journals;
using Applications.Requests.Transactions.Journals;
using Applications.Responses.ViewModels.Journals;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
namespace TrapchumApp.Components.Pages.Transactions.Journals
{
    public partial class JournalIndex
    {
        private readonly IJournalService _journalService;

        public JournalIndex(IJournalService journalService)
        {
            _journalService = journalService;
        }

        string SearchText = string.Empty;
        string types = string.Empty;
        int pageIndex = 1;
        int pageSize = 10;
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
        List<VJournal>? Journals { get; set; } = [];
        [Parameter] public int selectedIndex { get; set; } = 0;
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }
        List<int> SelectedRow = [10, 20, 50, 100];
        IEnumerable<VJournal> JournalItems = [];
        JournalEntryDto? modelDetails = new();
        void ShowCreatedTab()
        {
            if (!showCreateTab)
            {
                showCreateTab = true;
                indexCounts.Add("create");
                selectedIndex = 0;
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
        void RowSelecting() { }
        async void ChangedPageSize(int page)
        {
            pageSize = page;
            Console.WriteLine(page);
            await GetJournals();
        }

        protected override async Task OnInitializedAsync()
        {
            indexCounts.Add("index");
            await GetJournals();
            var resp = await _journalService.FindJournalTypesAsync(null);
            if (resp != null)
            {
                JournalTypes = resp.AdaptMany<JournalTypeDto>();
            }
            StateHasChanged();
        }
        protected async Task GetJournals(string? filter = null)
        {
            isDataLoading = true;
            try
            {
                var journalItems = await _journalService.FindJournalsAsync(pageIndex, pageSize, sortColumn, sortDescending, filter);
                counts = journalItems.Count;
                JournalItems = journalItems.Items;
                isDataLoading = false;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
                isDataLoading = false;
            }
            finally
            {
                isDataLoading = false;
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
                string dir = e.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
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
        async void ToDetails(long? e)
        {
            ContextMenuService.Close();
            try
            {
                var resp = await _journalService.FindJournalByIdAsync(e);
                if (resp != null)
                {
                    modelDetails = resp.Adapt<JournalEntryDto>();
                }
                showDetailTab = true;
                indexCounts.Add("detail");
                selectedIndex = 0;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
                Console.WriteLine(ex.InnerException!.Message);
            }

        }
    }
}


