using Microsoft.AspNetCore.Components.Web;

namespace Web.UI.Components.Comps;

public partial class MyTable<TItem> : ComponentBase
{
    // ── Data ─────────────────────────────────────────────────────────────
    [Parameter] public IEnumerable<TItem> Data { get; set; } = [];

    // ── Columns ───────────────────────────────────────────────────────────
    [Parameter] public RenderFragment? Columns { get; set; }

    // ── Toolbar ───────────────────────────────────────────────────────────
    [Parameter] public RenderFragment? ToolbarContent { get; set; }

    // ── Selection ─────────────────────────────────────────────────────────
    [Parameter] public HashSet<TItem> SelectedItems { get; set; } = [];
    [Parameter] public EventCallback<HashSet<TItem>> SelectedItemsChanged { get; set; }

    // ── Row events ────────────────────────────────────────────────────────
    [Parameter] public EventCallback<TItem> RowClick { get; set; }

    // ── Appearance ────────────────────────────────────────────────────────
    [Parameter] public bool Striped { get; set; } = true;
    [Parameter] public bool Bordered { get; set; } = true;
    [Parameter] public bool Compact { get; set; } = true;
    [Parameter] public bool ShowSearch { get; set; } = true;
    [Parameter] public string SearchPlaceholder { get; set; } = "Search...";
    [Parameter] public string EmptyMessage { get; set; } = "No data available.";
    [Parameter] public string? Height { get; set; }
    [Parameter] public Func<TItem, string?>? RowClass { get; set; }

    // ── Internal ──────────────────────────────────────────────────────────
    internal List<MyTableColumn<TItem>> _columns = [];
    private HashSet<TItem> _selectedItems = [];
    private string _searchText = string.Empty;
    private string? _height => Height;
    private bool _isDragging;

    // ── Column registration ───────────────────────────────────────────────
    internal void AddColumn(MyTableColumn<TItem> col)
    {
        if (!_columns.Contains(col))
        {
            _columns.Add(col);
            StateHasChanged();
        }
    }

    internal void RemoveColumn(MyTableColumn<TItem> col)
    {
        _columns.Remove(col);
        StateHasChanged();
    }

    // ── Lifecycle ─────────────────────────────────────────────────────────
    protected override void OnParametersSet()
    {
        _selectedItems = SelectedItems ?? [];
    }

    // ── Computed
    private IEnumerable<TItem> FilteredItems
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_searchText))

                return Data ?? [];

            var lower = _searchText.ToLowerInvariant();

            return (Data ?? []).Where(item =>
                item?.ToString()?.Contains(lower, StringComparison.OrdinalIgnoreCase) == true);
        }
    }

    private bool IsAllSelected
    {
        get
        {
            var list = FilteredItems.ToList();
            return list.Count > 0 && list.All(i => _selectedItems.Contains(i));
        }
    }

    // ── Selection helpers ─────────────────────────────────────────────────
    private async Task NotifySelectionChanged()
    {
        SelectedItems = _selectedItems;

        await SelectedItemsChanged.InvokeAsync(_selectedItems);

    }

    private async Task ClearSelection()
    {
        _selectedItems.Clear();

        await NotifySelectionChanged();
    }

    private async Task OnSelectAllChanged(ChangeEventArgs e)
    {
        var check = e.Value is bool b && b;
        if (check)
            foreach (var item in FilteredItems)
                _selectedItems.Add(item);
        else
            foreach (var item in FilteredItems)
                _selectedItems.Remove(item);

        await NotifySelectionChanged();
    }

    private async Task OnRowCheckboxChanged(ChangeEventArgs e, TItem item, int rowIndex)
    {
        var check = e.Value is bool b && b;

        if (check)
        {
            ApplyTopAnchoredRange(rowIndex);
        }
        else
        {
            var items = FilteredItems.ToList();

            for (var i = rowIndex; i < items.Count; i++)

                _selectedItems.Remove(items[i]);
        }

        await NotifySelectionChanged();
    }

    private async Task OnRowClick(MouseEventArgs e, TItem item)
    {
        if (!_isDragging)
            await RowClick.InvokeAsync(item);
    }

    // ── Drag-select ─────────────────────────────────────────────────────
    private void ApplyTopAnchoredRange(int toIndex)
    {
        var items = FilteredItems.ToList();
        _selectedItems.Clear();
        for (var i = 0; i <= toIndex && i < items.Count; i++)
            _selectedItems.Add(items[i]);
    }

    private async Task OnRowMouseDown(MouseEventArgs e, int rowIndex)
    {
        if (e.Button != 0) return;

        _isDragging = true;

        ApplyTopAnchoredRange(rowIndex);

        await NotifySelectionChanged();
    }

    private async Task OnRowMouseEnter(int rowIndex)
    {
        if (!_isDragging) return;

        ApplyTopAnchoredRange(rowIndex);

        await NotifySelectionChanged();
    }

    private async Task OnRowMouseUp()
    {
        if (_isDragging)
        {
            _isDragging = false;

            await NotifySelectionChanged();
        }
    }

    // ── Utilities
    private static string BuildThStyle(MyTableColumn<TItem> col)
    {
        var parts = new List<string> { $"text-align:{col.TextAlign.ToCss()}" };

        if (!string.IsNullOrEmpty(col.Width)) parts.Add($"width:{col.Width}");

        return string.Join(";", parts);
    }

    private static string? GetPropertyValue(TItem item, string title)
    {
        if (item is null) return null;

        var prop = typeof(TItem).GetProperty(title);

        return prop?.GetValue(item)?.ToString();
    }

    }
