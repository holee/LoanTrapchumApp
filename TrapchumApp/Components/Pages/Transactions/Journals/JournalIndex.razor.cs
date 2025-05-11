using Applications.Extensions.mappers;
using Applications.Interfaces.Transactions;
using Applications.Requests.Journals;
using Applications.Requests.Transactions.Journals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Applications.Services.Transactions.Journals;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Journals
{
    public partial class JournalIndex
    {
        [Parameter] public int? BranchId { get; set; } = 0;
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
        List<JournalEntryVm>? Journals { get; set; } = [];
        List<int> SelectedRow = [10, 20, 50, 100];
        IEnumerable<JournalEntryVm> JournalItems = [];
        JournalEntryVm? modelDetails = new();
        private string? _selectedJournalId;

        void ShowCreatedTab()
        {
            if (!showCreateTab)
            {
                showCreateTab = true;
                selectedIndex = 1;
                return;
            }

            selectedIndex = 0;
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
            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, filter);

            isDataLoading = true;

            try
            {
                var journalItems = await _journalService.GetJournalsAsync(BranchId,param);

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