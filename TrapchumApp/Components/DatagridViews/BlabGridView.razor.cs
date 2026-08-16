using Microsoft.JSInterop;
using System.Reflection;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TrapchumApp.Components.DatagridViews;

public partial class BlabGridView<TItem> : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    // ── Data ────────────────────────────────────────────────────────
    /// <summary>The data source to display.</summary>
    [Parameter] public IEnumerable<TItem>? Data { get; set; }

    /// <summary>Total count of items (for server-side paging).</summary>
    [Parameter] public int TotalCount { get; set; }

    /// <summary>Property name used to uniquely identify each row (like WebForms DataKeyNames).</summary>
    [Parameter] public string? DataKeyProperty { get; set; }

    // ── Columns ─────────────────────────────────────────────────────
    /// <summary>Column definitions via child content.</summary>
    [Parameter] public RenderFragment? Columns { get; set; }

    /// <summary>Custom toolbar content rendered in the header.</summary>
    [Parameter] public RenderFragment? ToolbarContent { get; set; }

    /// <summary>When true, columns are auto-generated from TItem's public properties if no Columns are defined.</summary>
    [Parameter] public bool AutoGenerateColumns { get; set; }

    // ── Footer ──────────────────────────────────────────────────────
    /// <summary>Custom footer summary row rendered in tfoot. Receives the current page items.</summary>
    [Parameter] public RenderFragment<IEnumerable<TItem>>? FooterSummary { get; set; }

    // ── Empty Data ──────────────────────────────────────────────────
    /// <summary>Custom template displayed when no data is available (like WebForms EmptyDataTemplate).</summary>
    [Parameter] public RenderFragment? EmptyDataTemplate { get; set; }

    // ── Paging ──────────────────────────────────────────────────────
    /// <summary>Enable or disable paging.</summary>
    [Parameter] public bool AllowPaging { get; set; } = true;

    /// <summary>Number of items per page.</summary>
    [Parameter] public int PageSize { get; set; } = 15;

    /// <summary>Available page size options.</summary>
    [Parameter] public int[] PageSizeOptions { get; set; } = [10, 15, 25, 50, 100];

    /// <summary>Where to render pagination controls.</summary>
    [Parameter] public GridViewPagerPosition PagerPosition { get; set; } = GridViewPagerPosition.Bottom;

    // ── Sorting ─────────────────────────────────────────────────────
    /// <summary>Enable or disable sorting.</summary>
    [Parameter] public bool AllowSorting { get; set; } = true;

    // ── Search / Filter ─────────────────────────────────────────────
    /// <summary>Placeholder for the global search box.</summary>
    [Parameter] public string SearchPlaceholder { get; set; } = "Search...";

    // ── Server callback ─────────────────────────────────────────────
    /// <summary>Callback for server-side data loading (paging, sorting, filtering).</summary>
    [Parameter] public EventCallback<GridViewLoadDataArgs> LoadData { get; set; }

    // ── Row events ──────────────────────────────────────────────────
    /// <summary>Fired when a row is clicked.</summary>
    [Parameter] public EventCallback<TItem> RowClick { get; set; }

    /// <summary>Fired when a row is double-clicked.</summary>
    [Parameter] public EventCallback<TItem> RowDoubleClick { get; set; }

    // ── Command events (WebForms style) ─────────────────────────────
    /// <summary>Fired for any row command (Edit, Delete, Select, custom).</summary>
    [Parameter] public EventCallback<GridViewCommandEventArgs<TItem>> RowCommand { get; set; }

    /// <summary>Fired when Edit is clicked on a row.</summary>
    [Parameter] public EventCallback<GridViewEditEventArgs<TItem>> RowEditing { get; set; }

    /// <summary>Fired when Update is clicked on an editing row.</summary>
    [Parameter] public EventCallback<GridViewEditEventArgs<TItem>> RowUpdating { get; set; }

    /// <summary>Fired when Cancel is clicked on an editing row.</summary>
    [Parameter] public EventCallback<GridViewEditEventArgs<TItem>> RowCancelingEdit { get; set; }

    /// <summary>Fired when Delete is clicked on a row.</summary>
    [Parameter] public EventCallback<GridViewDeleteEventArgs<TItem>> RowDeleting { get; set; }

    /// <summary>Fired when selection changes.</summary>
    [Parameter] public EventCallback<IEnumerable<TItem>> SelectedIndexChanged { get; set; }

    /// <summary>Fired when sort changes.</summary>
    [Parameter] public EventCallback<GridViewSortEventArgs> Sorting { get; set; }

    /// <summary>Fired when page changes.</summary>
    [Parameter] public EventCallback<GridViewPageEventArgs> PageIndexChanging { get; set; }

    // ── Row / Cell conditional appearance ────────────────────────────
    /// <summary>Returns a CSS class string for each row.</summary>
    [Parameter] public Func<TItem, string?>? RowClass { get; set; }

    /// <summary>Returns an inline style string for each row.</summary>
    [Parameter] public Func<TItem, string?>? RowStyle { get; set; }

    /// <summary>Returns a CSS class for each cell based on item and column.</summary>
    [Parameter] public Func<TItem, BlabGridViewColumn<TItem>, string?>? CellClass { get; set; }

    /// <summary>Returns an inline style for each cell based on item and column.</summary>
    [Parameter] public Func<TItem, BlabGridViewColumn<TItem>, string?>? CellStyle { get; set; }

    // ── Appearance ──────────────────────────────────────────────────
    [Parameter] public bool Striped { get; set; } = true;
    [Parameter] public bool Bordered { get; set; } = true;
    [Parameter] public bool Hoverable { get; set; } = true;
    [Parameter] public bool Compact { get; set; } = true;
    [Parameter] public bool ShowPageSizeSelector { get; set; } = true;
    [Parameter] public bool ShowSearchBox { get; set; } = true;
    [Parameter] public string? Height { get; set; }

    /// <summary>When true, the header row stays fixed while the body scrolls.</summary>
    [Parameter] public bool StickyHeader { get; set; }

    /// <summary>When true, the table collapses into a card/stacked layout on small screens.</summary>
    [Parameter] public bool Responsive { get; set; } = true;

    /// <summary>When true, a column visibility picker is shown in the toolbar.</summary>
    [Parameter] public bool ShowColumnFilterPicker { get; set; }

    // ── Edit Mode ───────────────────────────────────────────────────
    /// <summary>Inline edit mode: None, Row, or Cell.</summary>
    [Parameter] public GridViewEditMode EditMode { get; set; } = GridViewEditMode.Row;

    // ── Selection ───────────────────────────────────────────────────
    /// <summary>Row selection mode: None, Single, or Multiple.</summary>
    [Parameter] public GridViewSelectionMode SelectionMode { get; set; } = GridViewSelectionMode.None;

    /// <summary>Currently selected items (two-way bindable).</summary>
    [Parameter] public IEnumerable<TItem>? SelectedItems { get; set; }

    /// <summary>Callback when selected items change.</summary>
    [Parameter] public EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }

    // ── Row Details / Child Rows ────────────────────────────────────
    /// <summary>Expandable detail template rendered below the row when expanded.</summary>
    [Parameter] public RenderFragment<TItem>? DetailTemplate { get; set; }

    // ── Row Grouping ────────────────────────────────────────────────
    /// <summary>Groups rows by the returned key.</summary>
    [Parameter] public Func<TItem, string?>? GroupBy { get; set; }

    /// <summary>Custom group header template.</summary>
    [Parameter] public RenderFragment<GridViewGroupContext<TItem>>? GroupHeaderTemplate { get; set; }

    // ── Export / Print / Copy ────────────────────────────────────────
    /// <summary>Show CSV export button.</summary>
    [Parameter] public bool ShowExportButton { get; set; }

    /// <summary>Show print button.</summary>
    [Parameter] public bool ShowPrintButton { get; set; }

    /// <summary>Show copy-to-clipboard button.</summary>
    [Parameter] public bool ShowCopyButton { get; set; }

    /// <summary>Filename for CSV export (without extension).</summary>
    [Parameter] public string ExportFileName { get; set; } = "export";

    // ── Column Reorder & Resize ─────────────────────────────────────
    /// <summary>Allow drag-and-drop column reordering.</summary>
    [Parameter] public bool AllowColumnReorder { get; set; }

    /// <summary>Allow column resize by dragging.</summary>
    [Parameter] public bool AllowColumnResize { get; set; }

    // ── Delete confirmation ─────────────────────────────────────────
    /// <summary>Confirmation message shown before deleting a row.</summary>
    [Parameter] public string DeleteConfirmMessage { get; set; } = "Are you sure you want to delete this row?";

    /// <summary>Whether to show a confirmation dialog before deletion.</summary>
    [Parameter] public bool ShowDeleteConfirm { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    // ═══════════════════════════════════════════════════════════════
    // Internal state
    // ═══════════════════════════════════════════════════════════════
    internal List<BlabGridViewColumn<TItem>> ColumnDefinitions { get; } = [];
    private List<BlabGridViewColumn<TItem>>? _autoGeneratedColumns;
    private List<TItem> _items = [];
    private int _pageIndex;
    private int _pageSize;
    private bool _pageSizeInitialized;
    private string _searchText = string.Empty;
    private bool _isLoading;
    private Timer? _debounceTimer;
    private Timer? _columnFilterDebounceTimer;
    private IJSObjectReference? _jsModule;
    private bool _columnPickerOpen;
    private bool _deleteConfirmOpen;
    private TItem? _pendingDeleteItem;
    private int _pendingDeleteIndex;
    private string _gridId = $"bgv-{Guid.NewGuid():N}";

    // ── Multi-sort state ────────────────────────────────────────────
    private readonly List<GridViewSortInfo> _sortColumns = [];
    private string? _sortColumn => _sortColumns.Count > 0 ? _sortColumns[0].Column : null;
    private bool _sortDescending => _sortColumns.Count > 0 && _sortColumns[0].Descending;

    // ── Per-column filter state ─────────────────────────────────────
    private readonly Dictionary<string, string> _columnFilters = new();

    // ── Edit state ──────────────────────────────────────────────────
    private int _editIndex = -1;
    private TItem? _editingItem;
    private BlabGridViewColumn<TItem>? _editColumn;

    // ── Selection state ─────────────────────────────────────────────
    private readonly HashSet<TItem> _selectedItems = new();

    // ── Row detail state ────────────────────────────────────────────
    private readonly HashSet<TItem> _expandedRows = new();

    // ── Column reorder state ────────────────────────────────────────
    private int _dragColumnIndex = -1;

    // ═══════════════════════════════════════════════════════════════
    // Computed properties
    // ═══════════════════════════════════════════════════════════════
    internal IReadOnlyList<BlabGridViewColumn<TItem>> VisibleColumns =>
        EffectiveColumns.Where(c => c.Visible).ToList();

    internal IReadOnlyList<BlabGridViewColumn<TItem>> EffectiveColumns =>
        ColumnDefinitions.Count > 0 ? ColumnDefinitions : (_autoGeneratedColumns ?? []);

    private int TotalPages => _pageSize > 0 ? (int)Math.Ceiling((double)TotalCount / _pageSize) : 1;
    private int ShowingFrom => TotalCount == 0 ? 0 : _pageIndex * _pageSize + 1;
    private int ShowingTo => Math.Min((_pageIndex + 1) * _pageSize, TotalCount);

    private int ColSpan
    {
        get
        {
            int count = VisibleColumns.Count;
            if (HasSelection) count++;
            if (HasDetailTemplate) count++;
            return count;
        }
    }

    private bool ShowTopPagination => PagerPosition is GridViewPagerPosition.Top or GridViewPagerPosition.Both;
    private bool ShowBottomPagination => PagerPosition is GridViewPagerPosition.Bottom or GridViewPagerPosition.Both;
    private bool HasSelection => SelectionMode != GridViewSelectionMode.None;
    private bool HasDetailTemplate => DetailTemplate is not null;
    private bool HasGrouping => GroupBy is not null;
    private bool HasColumnFilters => EffectiveColumns.Any(c => c.Filterable);
    private bool HasToolbarButtons => ShowExportButton || ShowPrintButton || ShowCopyButton;
    private bool HasFooterTemplates => EffectiveColumns.Any(c => c.FooterTemplate is not null);

    private bool IsAllSelected =>
        _items.Count > 0 && _items.All(item => _selectedItems.Contains(item));

    // ═══════════════════════════════════════════════════════════════
    // Lifecycle
    // ═══════════════════════════════════════════════════════════════
    protected override void OnParametersSet()
    {
        if (!_pageSizeInitialized)
        {
            _pageSize = PageSize;
            _pageSizeInitialized = true;
        }

        if (Data is not null)
            _items = Data.ToList();

        // Sync external SelectedItems
        if (SelectedItems is not null && SelectionMode != GridViewSelectionMode.None)
        {
            _selectedItems.Clear();
            foreach (var item in SelectedItems)
                _selectedItems.Add(item);
        }

        // Auto-generate columns if needed
        if (AutoGenerateColumns && ColumnDefinitions.Count == 0)
            GenerateAutoColumns();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./Components/DatagridViews/BlabGridView.razor.js");

            await RequestData();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Auto-generate columns from TItem properties
    // ═══════════════════════════════════════════════════════════════
    private void GenerateAutoColumns()
    {
        _autoGeneratedColumns = [];
        var properties = typeof(TItem).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            if (!prop.CanRead) continue;

            var col = new BlabGridViewColumn<TItem>();

            // Set parameters via reflection on the component
            col.DataField = prop.Name;
            col.HeaderText = SplitCamelCase(prop.Name);
            col.Sortable = true;
            col.SortExpression = prop.Name;

            // Determine field type
            if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                col.FieldType = GridViewFieldType.CheckBoxField;
            }
            else
            {
                col.FieldType = GridViewFieldType.BoundField;
            }

            // Format strings for common types
            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                col.DataFormatString = "{0:dd/MM/yyyy}";
            }
            else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?)
                || prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            {
                col.DataFormatString = "{0:N2}";
                col.TextAlign = "right";
            }

            _autoGeneratedColumns.Add(col);
        }
    }

    private static string SplitCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var result = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            if (i > 0 && char.IsUpper(input[i]) && !char.IsUpper(input[i - 1]))
                result.Append(' ');
            result.Append(input[i]);
        }
        return result.ToString();
    }

    // ═══════════════════════════════════════════════════════════════
    // Column registration (called by child BlabGridViewColumn)
    // ═══════════════════════════════════════════════════════════════
    internal void AddColumn(BlabGridViewColumn<TItem> column)
    {
        if (!ColumnDefinitions.Contains(column))
        {
            ColumnDefinitions.Add(column);
            StateHasChanged();
        }
    }

    internal void RemoveColumn(BlabGridViewColumn<TItem> column)
    {
        ColumnDefinitions.Remove(column);
    }

    // ═══════════════════════════════════════════════════════════════
    // Property value helpers
    // ═══════════════════════════════════════════════════════════════
    internal static object? GetPropertyValueRaw(TItem item, string propertyName)
    {
        if (item is null) return null;
        var prop = typeof(TItem).GetProperty(propertyName);
        return prop?.GetValue(item);
    }

    internal static string? GetPropertyValue(TItem item, string propertyName)
    {
        return GetPropertyValueRaw(item, propertyName)?.ToString();
    }

    internal static string? GetFormattedValue(TItem item, string propertyName, string? formatString)
    {
        if (item is null) return null;
        var raw = GetPropertyValueRaw(item, propertyName);
        if (raw is null) return null;
        if (!string.IsNullOrEmpty(formatString))
        {
            return string.Format(formatString, raw);
        }
        return raw.ToString();
    }

    internal static bool GetBooleanValue(TItem item, string propertyName)
    {
        var raw = GetPropertyValueRaw(item, propertyName);
        if (raw is bool b) return b;
        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // Search (global)
    // ═══════════════════════════════════════════════════════════════
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

    // ═══════════════════════════════════════════════════════════════
    // Per-column filter
    // ═══════════════════════════════════════════════════════════════
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

    // ═══════════════════════════════════════════════════════════════
    // Sorting (multi-sort with Shift+Click)
    // ═══════════════════════════════════════════════════════════════
    private async Task OnSort(BlabGridViewColumn<TItem> column, MouseEventArgs? mouseArgs = null)
    {
        if (!AllowSorting || !column.Sortable) return;

        var sortProp = column.EffectiveSortProperty;
        if (string.IsNullOrEmpty(sortProp)) return;

        var isShift = mouseArgs?.ShiftKey ?? false;
        var existingIndex = _sortColumns.FindIndex(s => s.Column == sortProp);

        if (existingIndex >= 0)
        {
            var existing = _sortColumns[existingIndex];
            if (existing.Descending)
            {
                _sortColumns.RemoveAt(existingIndex);
            }
            else
            {
                _sortColumns[existingIndex] = new GridViewSortInfo
                {
                    Column = sortProp,
                    Descending = true
                };
            }
        }
        else
        {
            if (!isShift)
                _sortColumns.Clear();

            _sortColumns.Add(new GridViewSortInfo
            {
                Column = sortProp,
                Descending = false
            });
        }

        if (Sorting.HasDelegate)
        {
            await Sorting.InvokeAsync(new GridViewSortEventArgs
            {
                SortExpression = sortProp,
                Descending = _sortColumns.FirstOrDefault(s => s.Column == sortProp)?.Descending ?? false,
                SortColumns = _sortColumns.ToList()
            });
        }

        _pageIndex = 0;
        await RequestData();
    }

    private string GetSortCss(BlabGridViewColumn<TItem> column)
    {
        if (!AllowSorting || !column.Sortable || string.IsNullOrEmpty(column.EffectiveSortProperty))
            return "";

        var sortInfo = _sortColumns.FirstOrDefault(s => s.Column == column.EffectiveSortProperty);
        if (sortInfo is null) return "bgv__sortable";
        return sortInfo.Descending ? "bgv__sortable bgv__sort-desc" : "bgv__sortable bgv__sort-asc";
    }

    private string GetSortState(BlabGridViewColumn<TItem> column)
    {
        var sortInfo = _sortColumns.FirstOrDefault(s => s.Column == column.EffectiveSortProperty);
        if (sortInfo is null) return "none";
        return sortInfo.Descending ? "desc" : "asc";
    }

    private int GetSortOrder(BlabGridViewColumn<TItem> column)
    {
        if (_sortColumns.Count <= 1) return -1;
        var index = _sortColumns.FindIndex(s => s.Column == column.EffectiveSortProperty);
        return index >= 0 ? index + 1 : -1;
    }

    // ═══════════════════════════════════════════════════════════════
    // Pagination
    // ═══════════════════════════════════════════════════════════════
    private async Task GoToPage(int page)
    {
        if (page < 0 || page >= TotalPages) return;
        _pageIndex = page;

        if (PageIndexChanging.HasDelegate)
        {
            await PageIndexChanging.InvokeAsync(new GridViewPageEventArgs { NewPageIndex = page });
        }

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

    // ═══════════════════════════════════════════════════════════════
    // Row events
    // ═══════════════════════════════════════════════════════════════
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

    // ═══════════════════════════════════════════════════════════════
    // Command handling (WebForms style)
    // ═══════════════════════════════════════════════════════════════
    private async Task HandleCommand(string commandName, TItem item, int rowIndex)
    {
        // Fire generic RowCommand
        if (RowCommand.HasDelegate)
        {
            await RowCommand.InvokeAsync(new GridViewCommandEventArgs<TItem>
            {
                CommandName = commandName,
                Item = item,
                RowIndex = rowIndex
            });
        }

        switch (commandName)
        {
            case "Edit":
                await HandleEditCommand(item, rowIndex);
                break;
            case "Update":
                await HandleUpdateCommand(item, rowIndex);
                break;
            case "Cancel":
                await HandleCancelCommand(item, rowIndex);
                break;
            case "Delete":
                await HandleDeleteCommand(item, rowIndex);
                break;
            case "Select":
                await HandleSelectCommand(item);
                break;
        }
    }

    // ── Command wrapper methods (avoid escaped quotes in Razor) ─────
    private Task HandleCommandEdit(TItem item, int rowIndex) => HandleCommand("Edit", item, rowIndex);
    private Task HandleCommandUpdate(TItem item, int rowIndex) => HandleCommand("Update", item, rowIndex);
    private Task HandleCommandCancel(TItem item, int rowIndex) => HandleCommand("Cancel", item, rowIndex);
    private Task HandleCommandDelete(TItem item, int rowIndex) => HandleCommand("Delete", item, rowIndex);
    private Task HandleCommandSelect(TItem item, int rowIndex) => HandleCommand("Select", item, rowIndex);

    private async Task HandleEditCommand(TItem item, int rowIndex)
    {
        var args = new GridViewEditEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowEditing.HasDelegate)
            await RowEditing.InvokeAsync(args);

        if (!args.Cancel)
        {
            _editIndex = rowIndex;
            _editingItem = item;
            StateHasChanged();
        }
    }

    private async Task HandleUpdateCommand(TItem item, int rowIndex)
    {
        var args = new GridViewEditEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowUpdating.HasDelegate)
            await RowUpdating.InvokeAsync(args);

        if (!args.Cancel)
        {
            _editIndex = -1;
            _editingItem = default;
            StateHasChanged();
        }
    }

    private async Task HandleCancelCommand(TItem item, int rowIndex)
    {
        var args = new GridViewEditEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowCancelingEdit.HasDelegate)
            await RowCancelingEdit.InvokeAsync(args);

        _editIndex = -1;
        _editingItem = default;
        StateHasChanged();
    }

    private async Task HandleDeleteCommand(TItem item, int rowIndex)
    {
        if (ShowDeleteConfirm)
        {
            _pendingDeleteItem = item;
            _pendingDeleteIndex = rowIndex;
            _deleteConfirmOpen = true;
            StateHasChanged();
            return;
        }

        await PerformDelete(item, rowIndex);
    }

    private async Task ConfirmDelete()
    {
        _deleteConfirmOpen = false;
        if (_pendingDeleteItem is not null)
        {
            await PerformDelete(_pendingDeleteItem, _pendingDeleteIndex);
        }
        _pendingDeleteItem = default;
        StateHasChanged();
    }

    private void CancelDelete()
    {
        _deleteConfirmOpen = false;
        _pendingDeleteItem = default;
        StateHasChanged();
    }

    private async Task PerformDelete(TItem item, int rowIndex)
    {
        var args = new GridViewDeleteEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowDeleting.HasDelegate)
            await RowDeleting.InvokeAsync(args);

        if (!args.Cancel)
        {
            await RequestData();
        }
    }

    private async Task HandleSelectCommand(TItem item)
    {
        _selectedItems.Clear();
        _selectedItems.Add(item);
        await NotifySelectionChanged();
    }

    // ── Editing helpers ─────────────────────────────────────────────
    internal bool IsRowEditing(TItem item)
    {
        if (EditMode != GridViewEditMode.Row) return false;
        if (_editIndex < 0 || _editingItem is null) return false;
        return EqualityComparer<TItem>.Default.Equals(_editingItem, item);
    }

    internal bool IsCellEditing(TItem item, BlabGridViewColumn<TItem> col)
    {
        if (EditMode != GridViewEditMode.Cell) return false;
        if (_editIndex < 0 || _editingItem is null || _editColumn is null) return false;
        return EqualityComparer<TItem>.Default.Equals(_editingItem, item) && _editColumn == col;
    }

    private int GetRowIndex(TItem item) => _items.IndexOf(item);

    // ── Cell Editing commands ───────────────────────────────────────
    private async Task OnCellDoubleClick(TItem item, BlabGridViewColumn<TItem> col)
    {
        if (EditMode != GridViewEditMode.Cell || col.ReadOnly || string.IsNullOrEmpty(col.DataField)) return;

        var rowIndex = GetRowIndex(item);
        var args = new GridViewEditEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowEditing.HasDelegate)
            await RowEditing.InvokeAsync(args);

        if (!args.Cancel)
        {
            _editIndex = rowIndex;
            _editingItem = item;
            _editColumn = col;
            StateHasChanged();
        }
    }

    internal async Task CellEditCommit(TItem item)
    {
        if (EditMode != GridViewEditMode.Cell || _editingItem is null) return;
        
        var rowIndex = GetRowIndex(item);
        var args = new GridViewEditEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowUpdating.HasDelegate)
            await RowUpdating.InvokeAsync(args);

        if (!args.Cancel)
        {
            _editIndex = -1;
            _editingItem = default;
            _editColumn = null;
            StateHasChanged();
        }
    }

    internal async Task CellEditCancel(TItem item)
    {
        if (EditMode != GridViewEditMode.Cell || _editingItem is null) return;

        var rowIndex = GetRowIndex(item);
        var args = new GridViewEditEventArgs<TItem> { Item = item, RowIndex = rowIndex };

        if (RowCancelingEdit.HasDelegate)
            await RowCancelingEdit.InvokeAsync(args);

        _editIndex = -1;
        _editingItem = default;
        _editColumn = null;
        StateHasChanged();
    }

    private async Task HandleCellKeyPress(KeyboardEventArgs e, TItem item)
    {
        if (e.Key == "Enter")
        {
            await CellEditCommit(item);
        }
        else if (e.Key == "Escape")
        {
            await CellEditCancel(item);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // ButtonField command
    // ═══════════════════════════════════════════════════════════════
    private async Task HandleButtonFieldCommand(BlabGridViewColumn<TItem> col, TItem item)
    {
        var rowIndex = GetRowIndex(item);
        var commandName = col.CommandName ?? "ButtonClick";
        await HandleCommand(commandName, item, rowIndex);
    }

    // ═══════════════════════════════════════════════════════════════
    // Column picker
    // ═══════════════════════════════════════════════════════════════
    private void ToggleColumnPicker()
    {
        _columnPickerOpen = !_columnPickerOpen;
    }

    private void ToggleColumnVisibility(BlabGridViewColumn<TItem> column)
    {
        column.Visible = !column.Visible;
        StateHasChanged();
    }

    // ═══════════════════════════════════════════════════════════════
    // Row Selection
    // ═══════════════════════════════════════════════════════════════
    private async Task ToggleRowSelection(TItem item, ChangeEventArgs e)
    {
        var isChecked = bool.TryParse(e.Value?.ToString(), out var v) && v;

        if (SelectionMode == GridViewSelectionMode.Single)
        {
            _selectedItems.Clear();
            if (isChecked)
                _selectedItems.Add(item);
        }
        else if (SelectionMode == GridViewSelectionMode.Multiple)
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

        if (SelectedIndexChanged.HasDelegate)
            await SelectedIndexChanged.InvokeAsync(selected);

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

    // ═══════════════════════════════════════════════════════════════
    // Row Details / Expand
    // ═══════════════════════════════════════════════════════════════
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

    // ═══════════════════════════════════════════════════════════════
    // Row Grouping
    // ═══════════════════════════════════════════════════════════════
    private IEnumerable<IGrouping<string?, TItem>> GetGroupedItems()
    {
        if (GroupBy is null) return [];
        return _items.GroupBy(GroupBy);
    }

    // ═══════════════════════════════════════════════════════════════
    // Column Reorder (Drag & Drop)
    // ═══════════════════════════════════════════════════════════════
    private void OnColumnDragStart(int columnIndex)
    {
        if (!AllowColumnReorder) return;
        _dragColumnIndex = columnIndex;
    }

    private void OnColumnDragOver()
    {
        // Allow drop (handled via CSS)
    }

    private void OnColumnDrop(int targetIndex)
    {
        if (!AllowColumnReorder || _dragColumnIndex < 0 || _dragColumnIndex == targetIndex) return;

        var columns = ColumnDefinitions;
        if (_dragColumnIndex < columns.Count && targetIndex < columns.Count)
        {
            var col = columns[_dragColumnIndex];
            columns.RemoveAt(_dragColumnIndex);
            columns.Insert(targetIndex, col);
        }

        _dragColumnIndex = -1;
        StateHasChanged();
    }

    private void OnColumnDragEnd()
    {
        _dragColumnIndex = -1;
    }

    // ═══════════════════════════════════════════════════════════════
    // Export / Print / Copy
    // ═══════════════════════════════════════════════════════════════
    private async Task ExportCsv()
    {
        if (_jsModule is null) return;

        var visibleCols = VisibleColumns
            .Where(c => !string.IsNullOrEmpty(c.DataField) && !c.IsCommandType).ToList();
        var headers = visibleCols.Select(c => c.HeaderText).ToArray();
        var rows = _items.Select(item =>
            visibleCols.Select(c => GetFormattedValue(item, c.DataField!, c.DataFormatString) ?? "").ToArray()
        ).ToArray();

        await _jsModule.InvokeVoidAsync("exportCsv", headers, rows, ExportFileName);
    }

    private async Task CopyToClipboard()
    {
        if (_jsModule is null) return;

        var visibleCols = VisibleColumns
            .Where(c => !string.IsNullOrEmpty(c.DataField) && !c.IsCommandType).ToList();
        var headers = string.Join("\t", visibleCols.Select(c => c.HeaderText));
        var dataRows = _items.Select(item =>
            string.Join("\t", visibleCols.Select(c => GetFormattedValue(item, c.DataField!, c.DataFormatString) ?? ""))
        );
        var text = headers + "\n" + string.Join("\n", dataRows);

        await _jsModule.InvokeVoidAsync("copyToClipboard", text);
    }

    private async Task PrintTable()
    {
        if (_jsModule is null) return;

        var visibleCols = VisibleColumns
            .Where(c => !string.IsNullOrEmpty(c.DataField) && !c.IsCommandType).ToList();
        var headers = visibleCols.Select(c => c.HeaderText).ToArray();
        var rows = _items.Select(item =>
            visibleCols.Select(c => GetFormattedValue(item, c.DataField!, c.DataFormatString) ?? "").ToArray()
        ).ToArray();

        await _jsModule.InvokeVoidAsync("printTable", headers, rows);
    }

    // ═══════════════════════════════════════════════════════════════
    // Data request
    // ═══════════════════════════════════════════════════════════════
    private async Task RequestData()
    {
        if (!LoadData.HasDelegate) return;

        _isLoading = true;
        StateHasChanged();

        var args = new GridViewLoadDataArgs
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

    /// <summary>Reloads data from the server.</summary>
    public async Task ReloadAsync()
    {
        await RequestData();
    }

    /// <summary>Cancels the current edit and reloads data.</summary>
    public async Task CancelEditAsync()
    {
        _editIndex = -1;
        _editingItem = default;
        await RequestData();
    }

    // ═══════════════════════════════════════════════════════════════
    // Cleanup
    // ═══════════════════════════════════════════════════════════════
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
