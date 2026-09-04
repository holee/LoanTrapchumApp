using Applications.Features.CollateralValuations.Commands.DeleteCollateralValuation;
using Applications.Features.CollateralValuations.Dtos;
using Applications.Features.CollateralValuations.Queries.GetCollateralValuationsAsPaging;
using Applications.Interfaces;
using Radzen.Blazor;

namespace Web.UI.Pages.Lists.CollateralValuations;

public enum DynamicTabType { Create, Edit }

public record DynamicTab(string Id, string Title, string Icon, DynamicTabType Type, long? EntityId = null);

public partial class CollateralValuationIndex
{
    [Inject] private ICollateralValuationService _service { get; set; } = default!;

    private IEnumerable<CollateralValuationDto>? _items = [];
    private RadzenDataGrid<CollateralValuationDto> _gridRef = default!;
    private int _count = 0;
    private int _pageSize = 15;
    private int _skip = 0;
    private string _filterText = string.Empty;
    private bool _isLoading = false;
    private string _sortColumn = nameof(CollateralValuationDto.ModifiedDT);
    private bool _sortDescending = true;

    private int _selectedIndex = 0;
    private List<DynamicTab> _dynamicTabs = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadItems();
        await base.OnInitializedAsync();
    }

    async Task LoadData(LoadDataArgs args)
    {
        _skip = args.Skip ?? 0;
        _pageSize = args.Top ?? 15;
        _sortColumn = args.Sorts?.FirstOrDefault()?.Property ?? _sortColumn;
        _sortDescending = args.Sorts?.FirstOrDefault()?.SortOrder == SortOrder.Descending;
        await LoadItems();
    }

    async Task OnSearch(string filter)
    {
        _filterText = filter;
        _skip = 0;
        await LoadItems();
    }

    private async Task LoadItems()
    {
        _isLoading = true;
        try
        {
            var pageIndex = _pageSize > 0 ? _skip / _pageSize : 0;
            var param = new QueryParam(pageIndex, _pageSize, _sortColumn, false, _sortDescending, _filterText);
            var handler = new GetCollateralValuationsAsPagingQueryHandler(_service);
            var result = await handler.Handle(new GetCollateralValuationsAsPagingQuery { Param = param });
            _count = result.Count;
            _items = result.Items;
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    // ── Dynamic Tab Management ──

    void AddCreateTab()
    {
        var existing = _dynamicTabs.FindIndex(t => t.Type == DynamicTabType.Create);
        if (existing >= 0) { _selectedIndex = existing + 1; StateHasChanged(); return; }

        var tab = new DynamicTab(Guid.NewGuid().ToString("N"), "New Valuation", "add", DynamicTabType.Create);
        _dynamicTabs.Add(tab);
        _selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void OpenEditTab(CollateralValuationDto item)
    {
        ContextMenuService.Close();
        var existing = _dynamicTabs.FindIndex(t => t.EntityId == item.Id && t.Type == DynamicTabType.Edit);
        if (existing >= 0) { _selectedIndex = existing + 1; StateHasChanged(); return; }

        var tab = new DynamicTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: $"Edit {item.CollateralRefId ?? item.Id.ToString()}",
            Icon: "edit",
            Type: DynamicTabType.Edit,
            EntityId: item.Id);

        _dynamicTabs.Add(tab);
        _selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void CloseTab(string tabId)
    {
        var idx = _dynamicTabs.FindIndex(t => t.Id == tabId);
        if (idx < 0) return;
        _dynamicTabs.RemoveAt(idx);
        _selectedIndex = 0;
        StateHasChanged();
    }

    async Task OnTabClose(string tabId, bool saved)
    {
        CloseTab(tabId);
        if (saved) await LoadItems();
    }

    // ── Context Menu ──

    void ShowContextMenuWithItems(MouseEventArgs args, CollateralValuationDto item)
    {
        ContextMenuService.Open(args, BuildMenuItems(), e =>
        {
            ContextMenuService.Close();
            HandleAction(e.Value?.ToString(), item);
        });
    }

    void ShowCellContextMenu(DataGridCellMouseEventArgs<CollateralValuationDto> args)
    {
        if (args.Data is null) return;
        ContextMenuService.Open(args, BuildMenuItems(), e =>
        {
            ContextMenuService.Close();
            HandleAction(e.Value?.ToString(), args.Data);
        });
    }

    static List<ContextMenuItem> BuildMenuItems() =>
    [
        new ContextMenuItem { Text = "Edit",   Icon = "edit",   Value = "1" },
        new ContextMenuItem { Text = "Delete", Icon = "delete", Value = "2" },
    ];

    void HandleAction(string? value, CollateralValuationDto item)
    {
        switch (value)
        {
            case "1": OpenEditTab(item); break;
            case "2": _ = ConfirmDelete(item); break;
        }
    }

    async Task ConfirmDelete(CollateralValuationDto item)
    {
        var confirmed = await DialogService.Confirm(
            $"Are you sure you want to delete this valuation (ID: {item.Id})?",
            "Confirm Delete",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

        if (confirmed == true)
        {
            var handler = new DeleteCollateralValuationCommandHandler(_service);
            var result = await handler.Handle(new DeleteCollateralValuationCommand { Id = item.Id });
            if (!result.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Deleted", "Collateral valuation deleted.");
                await LoadItems();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", result.FirstError.Description);
            }
        }
    }
}
