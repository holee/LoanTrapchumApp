using Radzen.Blazor;
using Domains.Models.Identity;
using Applications.Features.Journals.Queries.GetJournalAsPaging;
using Applications.Features.Journals.Queries.GetJournalTypes;
using Applications.Features.Journals.Dtos;

namespace Web.UI.Pages.Transactions.Journals
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

        async Task EditJournal(JournalEntryVm j)
        {
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
                { "JournalId", j.JournalEntryId },
                { "BranchId", j.BranchId ?? BranchId },
                { "TransactionId", j.TransactionsId }
            };

            var dia = await DialogService.OpenAsync<JournalEntryUpdatePage>("Journal Entry", parameters, options);

            if (dia is bool success && success)
            {
                await GetJournals(SearchText);
            }
        }

        void ShowContextMenu(MouseEventArgs args, JournalEntryVm j)
        {
            ContextMenuService.Open(args, new List<ContextMenuItem>
            {
                new ContextMenuItem
                {
                    Text = "Journal Details",
                    Icon = "info",
                    Value = "1"
                },
                new ContextMenuItem
                {
                    Text = "Edit Journal",
                    Icon = "edit",
                    Value = "2"
                }
            }, (e) =>
            {
                ContextMenuService.Close();
                if (int.TryParse(e.Value?.ToString(), out int value))
                {
                    if (value == 1)
                    {
                        ShowDetailsTab(j);
                    }
                    else if (value == 2)
                    {
                        _ = EditJournal(j);
                    }
                }
            });
        }

        void OnMenuItemClick(MenuItemEventArgs args)
        {
            ContextMenuService.Close();
        }

        private void ShowContextMenu(DataGridCellMouseEventArgs<JournalEntryVm> args)
        {
            if (args.Data is null) return;

            ContextMenuService.Open(args, new List<ContextMenuItem>
            {
                new ContextMenuItem
                {
                    Text = "Journal Details",
                    Icon = "info",
                    Value = "1"
                },
                new ContextMenuItem
                {
                    Text = "Edit Journal",
                    Icon = "edit",
                    Value = "2"
                }
            }, (e) =>
            {
                ContextMenuService.Close();
                DoShowJournalActions(e, args);
            });
        }

        private void DoShowJournalActions(MenuItemEventArgs e, DataGridCellMouseEventArgs<JournalEntryVm> args)
        {
            if (args.Data is null) return;

            if (int.TryParse(e.Value?.ToString(), out int value))
            {
                if (value == 1)
                {
                    ShowDetailsTab(args.Data);
                    return;
                }
                if (value == 2)
                {
                    _ = EditJournal(args.Data);
                    return;
                }
            }
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