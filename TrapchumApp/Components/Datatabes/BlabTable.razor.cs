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

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    // ── Internal state ──────────────────────────────────────────────
    internal List<BlabTableColumn<TItem>> ColumnDefinitions { get; } = [];
    private List<TItem> _items = [];
    private int _pageIndex;
    private int _pageSize;
    private bool _pageSizeInitialized;
    private string _searchText = string.Empty;
    private string? _sortColumn;
    private bool _sortDescending;
    private bool _isLoading;
    private Timer? _debounceTimer;
    private IJSObjectReference? _jsModule;
    private bool _columnPickerOpen;

    // ── Edit state ──────────────────────────────────────────────────
    private TItem? _editingRow;
    private TItem? _editingCellRow;
    private BlabTableColumn<TItem>? _editingCellColumn;

    // ── Computed ─────────────────────────────────────────────────────
    private int TotalPages => _pageSize > 0 ? (int)Math.Ceiling((double)TotalCount / _pageSize) : 1;
    private int CurrentPage => _pageIndex + 1;
    private int ShowingFrom => TotalCount == 0 ? 0 : _pageIndex * _pageSize + 1;
    private int ShowingTo => Math.Min((_pageIndex + 1) * _pageSize, TotalCount);

    // Pagination position helpers
    private bool ShowTopPagination => PaginationPosition is BlabTablePaginationPosition.Top or BlabTablePaginationPosition.Both;
    private bool ShowBottomPagination => PaginationPosition is BlabTablePaginationPosition.Bottom or BlabTablePaginationPosition.Both;

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

    // ── Sorting ─────────────────────────────────────────────────────
    private async Task OnSort(BlabTableColumn<TItem> column)
    {
        if (!column.Sortable || string.IsNullOrEmpty(column.Property))
            return;

        if (_sortColumn == column.Property)
        {
            _sortDescending = !_sortDescending;
        }
        else
        {
            _sortColumn = column.Property;
            _sortDescending = false;
        }

        _pageIndex = 0;
        await RequestData();
    }

    private string GetSortCss(BlabTableColumn<TItem> column)
    {
        if (!column.Sortable || string.IsNullOrEmpty(column.Property))
            return "";

        if (_sortColumn != column.Property)
            return "blab-dt__sortable";

        return _sortDescending
            ? "blab-dt__sortable blab-dt__sort-desc"
            : "blab-dt__sortable blab-dt__sort-asc";
    }

    private string GetSortState(BlabTableColumn<TItem> column)
    {
        if (_sortColumn != column.Property) return "none";
        return _sortDescending ? "desc" : "asc";
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
    public string? SortColumn { get; init; }
    public bool SortDescending { get; init; }
    public int PageIndex { get; init; }
}

/// <summary>Arguments for cell edit events.</summary>
public sealed class BlabTableCellEditArgs<TItem>(TItem item, string? property)
{
    public TItem Item { get; } = item;
    public string? Property { get; } = property;
}
