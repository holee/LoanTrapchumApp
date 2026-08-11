using Microsoft.JSInterop;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TrapchumApp.Components.Datatabes;

/// <summary>Edit mode for BlabTable.</summary>
public enum BlabTableEditMode
{
    /// <summary>No inline editing.</summary>
    None,
    /// <summary>Click a single cell to edit it.</summary>
    Cell,
    /// <summary>Edit an entire row at once.</summary>
    Row
}

/// <summary>Controls where the pagination controls are rendered.</summary>
public enum BlabTablePaginationPosition
{
    /// <summary>Pagination at the bottom only (default).</summary>
    Bottom,
    /// <summary>Pagination at the top only.</summary>
    Top,
    /// <summary>Pagination at both top and bottom.</summary>
    Both
}

public partial class BlabTable<TItem> : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    // ── Data ────────────────────────────────────────────────────────
    [Parameter] public IEnumerable<TItem>? Data { get; set; }
    [Parameter] public int TotalCount { get; set; }

    // ── Columns ─────────────────────────────────────────────────────
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public RenderFragment? ToolbarContent { get; set; }

    // ── Footer summary ──────────────────────────────────────────────
    /// <summary>
    /// Custom footer summary row rendered in a &lt;tfoot&gt; section.
    /// Receives the current page items as context.
    /// </summary>
    [Parameter] public RenderFragment<IEnumerable<TItem>>? FooterSummary { get; set; }

    // ── Pagination ──────────────────────────────────────────────────
    [Parameter] public int PageSize { get; set; } = 15;
    [Parameter] public int[] PageSizeOptions { get; set; } = [10, 15, 25, 50, 100];

    /// <summary>Where to render pagination controls.</summary>
    [Parameter] public BlabTablePaginationPosition PaginationPosition { get; set; } = BlabTablePaginationPosition.Bottom;

    // ── Search / filter ─────────────────────────────────────────────
    [Parameter] public string SearchPlaceholder { get; set; } = "Search...";

    // ── Server callback ─────────────────────────────────────────────
    [Parameter] public EventCallback<BlabTableLoadDataArgs> LoadData { get; set; }

    // ── Row events ──────────────────────────────────────────────────
    [Parameter] public EventCallback<TItem> RowClick { get; set; }
    [Parameter] public EventCallback<TItem> RowDoubleClick { get; set; }

    // ── Editing ─────────────────────────────────────────────────────
    [Parameter] public BlabTableEditMode EditMode { get; set; } = BlabTableEditMode.None;
    [Parameter] public EventCallback<TItem> RowEditSave { get; set; }
    [Parameter] public EventCallback<TItem> RowEditCancel { get; set; }
    [Parameter] public EventCallback<BlabTableCellEditArgs<TItem>> CellEditSave { get; set; }
    [Parameter] public EventCallback<BlabTableCellEditArgs<TItem>> CellEditCancel { get; set; }

    // ── Row / Cell conditional appearance ────────────────────────────
    /// <summary>
    /// Returns a CSS class string for each row based on the item. Applied to the &lt;tr&gt;.
    /// </summary>
    [Parameter] public Func<TItem, string?>? RowClass { get; set; }

    /// <summary>
    /// Returns an inline style string for each row based on the item. Applied to the &lt;tr&gt;.
    /// </summary>
    [Parameter] public Func<TItem, string?>? RowStyle { get; set; }

    /// <summary>
    /// Returns a CSS class string for each cell based on the item and column. Applied to the &lt;td&gt;.
    /// </summary>
    [Parameter] public Func<TItem, BlabTableColumn<TItem>, string?>? CellClass { get; set; }

    /// <summary>
    /// Returns an inline style string for each cell based on the item and column. Applied to the &lt;td&gt;.
    /// </summary>
    [Parameter] public Func<TItem, BlabTableColumn<TItem>, string?>? CellStyle { get; set; }

    // ── Appearance ──────────────────────────────────────────────────
    [Parameter] public bool Striped { get; set; } = true;
    [Parameter] public bool Bordered { get; set; } = true;
    [Parameter] public bool Hoverable { get; set; } = true;
    [Parameter] public bool Compact { get; set; } = true;
    [Parameter] public bool ShowPageSizeSelector { get; set; } = true;
    [Parameter] public bool ShowSearchBox { get; set; } = true;
    [Parameter] public string? Height { get; set; }

    /// <summary>When true, the table header row stays fixed while the body scrolls (requires Height).</summary>
    [Parameter] public bool StickyHeader { get; set; }

    /// <summary>When true, the table collapses into a card/stacked layout on small screens.</summary>
    [Parameter] public bool Responsive { get; set; } = true;

    /// <summary>When true, a column visibility picker button is shown in the toolbar.</summary>
    [Parameter] public bool ShowColumnFilterPicker { get; set; }

    // ── Row Selection ───────────────────────────────────────────────
    /// <summary>Row selection mode: None, Single, or Multiple.</summary>
    [Parameter] public BlabTableSelectionMode SelectionMode { get; set; } = BlabTableSelectionMode.None;

    /// <summary>Currently selected items (two-way bindable).</summary>
    [Parameter] public IEnumerable<TItem>? SelectedItems { get; set; }

    /// <summary>Callback when selected items change.</summary>
    [Parameter] public EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }

    /// <summary>Fired when selection changes. Carries the full selected list.</summary>
    [Parameter] public EventCallback<IEnumerable<TItem>> SelectionChanged { get; set; }

    // ── Row Details / Child Rows ────────────────────────────────────
    /// <summary>
    /// Expandable detail template rendered below the row when expanded.
    /// Receives the row TItem as context.
    /// </summary>
    [Parameter] public RenderFragment<TItem>? DetailTemplate { get; set; }

    // ── Row Grouping ────────────────────────────────────────────────
    /// <summary>
    /// Groups rows by the returned key. A group header row is rendered before each group.
    /// </summary>
    [Parameter] public Func<TItem, string?>? GroupBy { get; set; }

    /// <summary>
    /// Custom group header template. Receives the group key and items as context.
    /// </summary>
    [Parameter] public RenderFragment<BlabTableGroupContext<TItem>>? GroupHeaderTemplate { get; set; }

    // ── Export / Print / Copy ────────────────────────────────────────
    /// <summary>Show CSV export button in toolbar.</summary>
    [Parameter] public bool ShowExportButton { get; set; }

    /// <summary>Show print button in toolbar.</summary>
    [Parameter] public bool ShowPrintButton { get; set; }

    /// <summary>Show copy-to-clipboard button in toolbar.</summary>
    [Parameter] public bool ShowCopyButton { get; set; }

    /// <summary>Filename for CSV export (without extension).</summary>
    [Parameter] public string ExportFileName { get; set; } = "export";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    // ── Internal state ──────────────────────────────────────────────
    internal List<BlabTableColumn<TItem>> ColumnDefinitions { get; } = [];
    private List<TItem> _items = [];
    private int _pageIndex;
    private int _pageSize;
    private bool _pageSizeInitialized;
    private string _searchText = string.Empty;
    private bool _isLoading;
    private Timer? _debounceTimer;
    private IJSObjectReference? _jsModule;
    private bool _columnPickerOpen;

    // ── Multi-sort state ────────────────────────────────────────────
    private readonly List<BlabTableSortInfo> _sortColumns = [];

    // Legacy single-sort computed properties for backward compat in args
    private string? _sortColumn => _sortColumns.Count > 0 ? _sortColumns[0].Column : null;
    private bool _sortDescending => _sortColumns.Count > 0 && _sortColumns[0].Descending;

    // ── Per-column filter state ─────────────────────────────────────
    private readonly Dictionary<string, string> _columnFilters = new();
    private Timer? _columnFilterDebounceTimer;

    // ── Edit state ──────────────────────────────────────────────────
    private TItem? _editingRow;
    private TItem? _editingCellRow;
    private BlabTableColumn<TItem>? _editingCellColumn;

    // ── Row selection state ──────────────────────────────────────────
    private readonly HashSet<TItem> _selectedItems = new();

    // ── Row detail state ────────────────────────────────────────────
    private readonly HashSet<TItem> _expandedRows = new();

    // ── Computed ─────────────────────────────────────────────────────
    private int TotalPages => _pageSize > 0 ? (int)Math.Ceiling((double)TotalCount / _pageSize) : 1;
    private int CurrentPage => _pageIndex + 1;
    private int ShowingFrom => TotalCount == 0 ? 0 : _pageIndex * _pageSize + 1;
    private int ShowingTo => Math.Min((_pageIndex + 1) * _pageSize, TotalCount);
    private int ColSpan => ColumnDefinitions.Count(c => c.Visible) + (HasSelection ? 1 : 0) + (HasDetailTemplate ? 1 : 0) + (EditMode == BlabTableEditMode.Row ? 1 : 0);

    // Pagination position helpers
    private bool ShowTopPagination => PaginationPosition is BlabTablePaginationPosition.Top or BlabTablePaginationPosition.Both;
    private bool ShowBottomPagination => PaginationPosition is BlabTablePaginationPosition.Bottom or BlabTablePaginationPosition.Both;

    // Selection helpers
    private bool HasSelection => SelectionMode != BlabTableSelectionMode.None;
    private bool HasDetailTemplate => DetailTemplate is not null;
    private bool HasGrouping => GroupBy is not null;
    private bool HasColumnFilters => ColumnDefinitions.Any(c => c.Filterable);
    private bool HasToolbarButtons => ShowExportButton || ShowPrintButton || ShowCopyButton;

    private bool IsAllSelected =>
        _items.Count > 0 && _items.All(item => _selectedItems.Contains(item));

    private bool IsIndeterminate =>
        _selectedItems.Count > 0 && !IsAllSelected && _items.Any(item => _selectedItems.Contains(item));

    // ── Lifecycle ───────────────────────────────────────────────────
    protected override void OnParametersSet()
    {
        if (!_pageSizeInitialized)
        {
            _pageSize = PageSize;
            _pageSizeInitialized = true;
        }

        if (Data is not null)
            _items = Data.ToList();

        // Sync external SelectedItems parameter
        if (SelectedItems is not null && SelectionMode != BlabTableSelectionMode.None)
        {
            _selectedItems.Clear();
            foreach (var item in SelectedItems)
                _selectedItems.Add(item);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./Components/Datatabes/BlabTable.razor.js");

            await RequestData();
        }
    }

    // ── Column registration (called by child BlabTableColumn) ───────
    internal void AddColumn(BlabTableColumn<TItem> column)
    {
        if (!ColumnDefinitions.Contains(column))
        {
            ColumnDefinitions.Add(column);
            StateHasChanged();
        }
    }

    internal void RemoveColumn(BlabTableColumn<TItem> column)
    {
        ColumnDefinitions.Remove(column);
    }

    // ── Search ──────────────────────────────────────────────────────
    private void OnSearchInput(ChangeEventArgs e)
    {
        _searchText = e.Value?.ToString() ?? string.Empty;

        _debounceTimer?.Stop();
        _debounceTimer?.Dispose();

        _debounceTimer = new Timer(350);
        _debounceTimer.Elapsed += DebounceElapsed;
        _debounceTimer.AutoReset = false;
        _debounceTimer.Start();
    }

    private async void DebounceElapsed(object? sender, ElapsedEventArgs e)
    {
        await InvokeAsync(async () =>
        {
            _pageIndex = 0;
            await RequestData();
            StateHasChanged();
        });
    }

    // ── Per-column filter ───────────────────────────────────────────
    private void OnColumnFilterInput(string property, ChangeEventArgs e)
    {
        var value = e.Value?.ToString() ?? string.Empty;

        if (string.IsNullOrEmpty(value))
            _columnFilters.Remove(property);
        else
            _columnFilters[property] = value;

        _columnFilterDebounceTimer?.Stop();
        _columnFilterDebounceTimer?.Dispose();

        _columnFilterDebounceTimer = new Timer(400);
        _columnFilterDebounceTimer.Elapsed += ColumnFilterDebounceElapsed;
        _columnFilterDebounceTimer.AutoReset = false;
        _columnFilterDebounceTimer.Start();
    }

    private async void ColumnFilterDebounceElapsed(object? sender, ElapsedEventArgs e)
    {
        await InvokeAsync(async () =>
        {
            _pageIndex = 0;
            await RequestData();
            StateHasChanged();
        });
    }

    private string GetColumnFilterValue(string? property)
    {
        if (property is null) return string.Empty;
        return _columnFilters.TryGetValue(property, out var val) ? val : string.Empty;
    }

    // ── Sorting (multi-sort with Shift+Click) ───────────────────────
    private async Task OnSort(BlabTableColumn<TItem> column, MouseEventArgs? mouseArgs = null)
    {
        if (!column.Sortable || string.IsNullOrEmpty(column.Property))
            return;

        var isShift = mouseArgs?.ShiftKey ?? false;

        var existingIndex = _sortColumns.FindIndex(s => s.Column == column.Property);

        if (existingIndex >= 0)
        {
            // Toggle direction or remove
            var existing = _sortColumns[existingIndex];
            if (existing.Descending)
            {
                // Already desc → remove from sort
                _sortColumns.RemoveAt(existingIndex);
            }
            else
            {
                // Asc → toggle to desc
                _sortColumns[existingIndex] = new BlabTableSortInfo
                {
                    Column = column.Property,
                    Descending = true
                };
            }
        }
        else
        {
            if (!isShift)
            {
                // Single sort: clear others
                _sortColumns.Clear();
            }

            _sortColumns.Add(new BlabTableSortInfo
            {
                Column = column.Property,
                Descending = false
            });
        }

        _pageIndex = 0;
        await RequestData();
    }

    private string GetSortCss(BlabTableColumn<TItem> column)
    {
        if (!column.Sortable || string.IsNullOrEmpty(column.Property))
            return "";

        var sortInfo = _sortColumns.FirstOrDefault(s => s.Column == column.Property);
        if (sortInfo is null)
            return "blab-dt__sortable";

        return sortInfo.Descending
            ? "blab-dt__sortable blab-dt__sort-desc"
            : "blab-dt__sortable blab-dt__sort-asc";
    }

    private string GetSortState(BlabTableColumn<TItem> column)
    {
        var sortInfo = _sortColumns.FirstOrDefault(s => s.Column == column.Property);
        if (sortInfo is null) return "none";
        return sortInfo.Descending ? "desc" : "asc";
    }

    private int GetSortOrder(BlabTableColumn<TItem> column)
    {
        if (_sortColumns.Count <= 1) return -1;
        var index = _sortColumns.FindIndex(s => s.Column == column.Property);
        return index >= 0 ? index + 1 : -1;
    }

    // ── Pagination ──────────────────────────────────────────────────
    private async Task GoToPage(int page)
    {
        if (page < 0 || page >= TotalPages) return;
        _pageIndex = page;
        await RequestData();
    }

    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            _pageSize = size;
            _pageIndex = 0;
            await RequestData();
        }
    }

    private IEnumerable<int> GetPageNumbers()
    {
        const int maxVisible = 5;

        if (TotalPages <= maxVisible)
        {
            for (int i = 0; i < TotalPages; i++)
                yield return i;
            yield break;
        }

        int start = Math.Max(0, _pageIndex - maxVisible / 2);
        int end = Math.Min(TotalPages - 1, start + maxVisible - 1);

        if (end - start < maxVisible - 1)
            start = Math.Max(0, end - maxVisible + 1);

        for (int i = start; i <= end; i++)
            yield return i;
    }

    // ── Row events ──────────────────────────────────────────────────
    private async Task OnRowClick(TItem item)
    {
        if (RowClick.HasDelegate)
            await RowClick.InvokeAsync(item);
    }

    private async Task OnRowDoubleClick(TItem item)
    {
        if (RowDoubleClick.HasDelegate)
            await RowDoubleClick.InvokeAsync(item);
    }

    // ── Row editing ─────────────────────────────────────────────────
    internal bool IsRowEditing(TItem item) =>
        EditMode == BlabTableEditMode.Row && _editingRow is not null && EqualityComparer<TItem>.Default.Equals(_editingRow, item);

    public void BeginEditRow(TItem item)
    {
        if (EditMode != BlabTableEditMode.Row) return;
        CancelAllEditing();
        _editingRow = item;
        StateHasChanged();
    }

    public async Task SaveEditRow()
    {
        if (_editingRow is null) return;
        var item = _editingRow;
        _editingRow = default;
        StateHasChanged();

        if (RowEditSave.HasDelegate)
            await RowEditSave.InvokeAsync(item);
    }

    public async Task CancelEditRow()
    {
        if (_editingRow is null) return;
        var item = _editingRow;
        _editingRow = default;
        StateHasChanged();

        if (RowEditCancel.HasDelegate)
            await RowEditCancel.InvokeAsync(item);
    }

    // ── Cell editing ────────────────────────────────────────────────
    internal bool IsCellEditing(TItem item, BlabTableColumn<TItem> column) =>
        EditMode == BlabTableEditMode.Cell
        && _editingCellRow is not null
        && _editingCellColumn == column
        && EqualityComparer<TItem>.Default.Equals(_editingCellRow, item);

    public void BeginEditCell(TItem item, BlabTableColumn<TItem> column)
    {
        if (EditMode != BlabTableEditMode.Cell || !column.Editable) return;
        CancelAllEditing();
        _editingCellRow = item;
        _editingCellColumn = column;
        StateHasChanged();
    }

    public async Task SaveEditCell()
    {
        if (_editingCellRow is null || _editingCellColumn is null) return;
        var args = new BlabTableCellEditArgs<TItem>(_editingCellRow, _editingCellColumn.Property);
        _editingCellRow = default;
        _editingCellColumn = null;
        StateHasChanged();

        if (CellEditSave.HasDelegate)
            await CellEditSave.InvokeAsync(args);
    }

    public async Task CancelEditCell()
    {
        if (_editingCellRow is null || _editingCellColumn is null) return;
        var args = new BlabTableCellEditArgs<TItem>(_editingCellRow, _editingCellColumn.Property);
        _editingCellRow = default;
        _editingCellColumn = null;
        StateHasChanged();

        if (CellEditCancel.HasDelegate)
            await CellEditCancel.InvokeAsync(args);
    }

    private void CancelAllEditing()
    {
        _editingRow = default;
        _editingCellRow = default;
        _editingCellColumn = null;
    }

    // ── Column filter picker ────────────────────────────────────────
    private void ToggleColumnPicker()
    {
        _columnPickerOpen = !_columnPickerOpen;
    }

    private void ToggleColumnVisibility(BlabTableColumn<TItem> column)
    {
        column.Visible = !column.Visible;
        StateHasChanged();
    }

    // ── Row Selection ───────────────────────────────────────────────
    private async Task ToggleRowSelection(TItem item, ChangeEventArgs e)
    {
        var isChecked = bool.TryParse(e.Value?.ToString(), out var v) && v;

        if (SelectionMode == BlabTableSelectionMode.Single)
        {
            _selectedItems.Clear();
            if (isChecked)
                _selectedItems.Add(item);
        }
        else if (SelectionMode == BlabTableSelectionMode.Multiple)
        {
            if (isChecked)
                _selectedItems.Add(item);
            else
                _selectedItems.Remove(item);
        }

        await NotifySelectionChanged();
    }

    private async Task ToggleSelectAll(ChangeEventArgs e)
    {
        var isChecked = bool.TryParse(e.Value?.ToString(), out var v) && v;

        if (isChecked)
        {
            foreach (var item in _items)
                _selectedItems.Add(item);
        }
        else
        {
            foreach (var item in _items)
                _selectedItems.Remove(item);
        }

        await NotifySelectionChanged();
    }

    private bool IsRowSelected(TItem item) => _selectedItems.Contains(item);

    private async Task NotifySelectionChanged()
    {
        var selected = _selectedItems.ToList();

        if (SelectedItemsChanged.HasDelegate)
            await SelectedItemsChanged.InvokeAsync(selected);

        if (SelectionChanged.HasDelegate)
            await SelectionChanged.InvokeAsync(selected);

        StateHasChanged();
    }

    /// <summary>Clears all selected items programmatically.</summary>
    public async Task ClearSelectionAsync()
    {
        _selectedItems.Clear();
        await NotifySelectionChanged();
    }

    /// <summary>Gets the currently selected items.</summary>
    public IReadOnlyCollection<TItem> GetSelectedItems() => _selectedItems;

    // ── Row Details / Expand ────────────────────────────────────────
    private void ToggleRowDetail(TItem item)
    {
        if (!_expandedRows.Remove(item))
            _expandedRows.Add(item);

        StateHasChanged();
    }

    private bool IsRowExpanded(TItem item) => _expandedRows.Contains(item);

    /// <summary>Expands all rows' detail views.</summary>
    public void ExpandAllDetails()
    {
        foreach (var item in _items)
            _expandedRows.Add(item);
        StateHasChanged();
    }

    /// <summary>Collapses all rows' detail views.</summary>
    public void CollapseAllDetails()
    {
        _expandedRows.Clear();
        StateHasChanged();
    }

    // ── Row Grouping ────────────────────────────────────────────────
    private IEnumerable<IGrouping<string?, TItem>> GetGroupedItems()
    {
        if (GroupBy is null) return [];
        return _items.GroupBy(GroupBy);
    }

    // ── Export / Print / Copy ────────────────────────────────────────
    private async Task ExportCsv()
    {
        if (_jsModule is null) return;

        var visibleCols = ColumnDefinitions.Where(c => c.Visible && !string.IsNullOrEmpty(c.Property)).ToList();
        var headers = visibleCols.Select(c => c.Title).ToArray();
        var rows = _items.Select(item =>
            visibleCols.Select(c => GetPropertyValue(item, c.Property!) ?? "").ToArray()
        ).ToArray();

        await _jsModule.InvokeVoidAsync("exportCsv", headers, rows, ExportFileName);
    }

    private async Task CopyToClipboard()
    {
        if (_jsModule is null) return;

        var visibleCols = ColumnDefinitions.Where(c => c.Visible && !string.IsNullOrEmpty(c.Property)).ToList();
        var headers = string.Join("\t", visibleCols.Select(c => c.Title));
        var dataRows = _items.Select(item =>
            string.Join("\t", visibleCols.Select(c => GetPropertyValue(item, c.Property!) ?? ""))
        );
        var text = headers + "\n" + string.Join("\n", dataRows);

        await _jsModule.InvokeVoidAsync("copyToClipboard", text);
    }

    private async Task PrintTable()
    {
        if (_jsModule is null) return;

        var visibleCols = ColumnDefinitions.Where(c => c.Visible && !string.IsNullOrEmpty(c.Property)).ToList();
        var headers = visibleCols.Select(c => c.Title).ToArray();
        var rows = _items.Select(item =>
            visibleCols.Select(c => GetPropertyValue(item, c.Property!) ?? "").ToArray()
        ).ToArray();

        await _jsModule.InvokeVoidAsync("printTable", headers, rows);
    }

    // ── Data request ────────────────────────────────────────────────
    private async Task RequestData()
    {
        if (!LoadData.HasDelegate) return;

        _isLoading = true;
        StateHasChanged();

        var args = new BlabTableLoadDataArgs
        {
            Skip = _pageIndex * _pageSize,
            Top = _pageSize,
            SearchText = _searchText,
            SortColumn = _sortColumn,
            SortDescending = _sortDescending,
            SortColumns = _sortColumns.ToList(),
            ColumnFilters = _columnFilters.Count > 0
                ? new Dictionary<string, string>(_columnFilters)
                : null,
            PageIndex = _pageIndex
        };

        await LoadData.InvokeAsync(args);

        _isLoading = false;
        StateHasChanged();
    }

    public async Task ReloadAsync()
    {
        await RequestData();
    }

    // ── Cleanup ─────────────────────────────────────────────────────
    public async ValueTask DisposeAsync()
    {
        _debounceTimer?.Stop();
        _debounceTimer?.Dispose();

        _columnFilterDebounceTimer?.Stop();
        _columnFilterDebounceTimer?.Dispose();

        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
        }
    }
}

/// <summary>Arguments passed to the LoadData callback.</summary>
public sealed class BlabTableLoadDataArgs
{
    public int Skip { get; init; }
    public int Top { get; init; }
    public string? SearchText { get; init; }

    /// <summary>Primary sort column (first in multi-sort, for backward compat).</summary>
    public string? SortColumn { get; init; }

    /// <summary>Primary sort direction (first in multi-sort, for backward compat).</summary>
    public bool SortDescending { get; init; }

    /// <summary>All sort columns in order (for multi-sort support).</summary>
    public List<BlabTableSortInfo>? SortColumns { get; init; }

    /// <summary>Per-column filter values. Key = Property name, Value = filter text.</summary>
    public Dictionary<string, string>? ColumnFilters { get; init; }

    public int PageIndex { get; init; }
}

/// <summary>Arguments for cell edit events.</summary>
public sealed class BlabTableCellEditArgs<TItem>(TItem item, string? property)
{
    public TItem Item { get; } = item;
    public string? Property { get; } = property;
}

/// <summary>Context for group header templates.</summary>
public sealed class BlabTableGroupContext<TItem>
{
    public string? GroupKey { get; init; }
    public IEnumerable<TItem> Items { get; init; } = [];
    public int Count { get; init; }
}
