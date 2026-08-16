namespace TrapchumApp.Components.DatagridViews;

/// <summary>Column field types mirroring ASP.NET WebForms GridView field types.</summary>
public enum GridViewFieldType
{
    /// <summary>Displays data as text from a property (like WebForms BoundField).</summary>
    BoundField,
    /// <summary>Uses custom Razor templates for full control (like WebForms TemplateField).</summary>
    TemplateField,
    /// <summary>Renders built-in Edit/Delete/Select command buttons (like WebForms CommandField).</summary>
    CommandField,
    /// <summary>Renders a clickable hyperlink (like WebForms HyperLinkField).</summary>
    HyperLinkField,
    /// <summary>Renders an image from a URL property (like WebForms ImageField).</summary>
    ImageField,
    /// <summary>Renders a checkbox for boolean properties (like WebForms CheckBoxField).</summary>
    CheckBoxField,
    /// <summary>Renders a clickable action button (like WebForms ButtonField).</summary>
    ButtonField
}

/// <summary>Row selection mode.</summary>
public enum GridViewSelectionMode
{
    /// <summary>No row selection.</summary>
    None,
    /// <summary>Single row selection (radio-style).</summary>
    Single,
    /// <summary>Multiple row selection with checkboxes.</summary>
    Multiple
}

/// <summary>Inline edit mode.</summary>
public enum GridViewEditMode
{
    /// <summary>No inline editing.</summary>
    None,
    /// <summary>Edit an entire row at once via CommandField.</summary>
    Row,
    /// <summary>Edit a single cell at a time.</summary>
    Cell
}

/// <summary>Where pagination controls are rendered.</summary>
public enum GridViewPagerPosition
{
    /// <summary>Bottom only (default).</summary>
    Bottom,
    /// <summary>Top only.</summary>
    Top,
    /// <summary>Both top and bottom.</summary>
    Both
}

/// <summary>Represents a single sort column in multi-sort mode.</summary>
public sealed class GridViewSortInfo
{
    public string Column { get; init; } = string.Empty;
    public bool Descending { get; init; }
}

/// <summary>Arguments passed to the LoadData callback for server-side data loading.</summary>
public sealed class GridViewLoadDataArgs
{
    public int Skip { get; init; }
    public int Top { get; init; }
    public string? SearchText { get; init; }

    /// <summary>Primary sort column (first in multi-sort, for backward compat).</summary>
    public string? SortColumn { get; init; }

    /// <summary>Primary sort direction (first in multi-sort, for backward compat).</summary>
    public bool SortDescending { get; init; }

    /// <summary>All sort columns in order (for multi-sort support).</summary>
    public List<GridViewSortInfo>? SortColumns { get; init; }

    /// <summary>Per-column filter values. Key = Property name, Value = filter text.</summary>
    public Dictionary<string, string>? ColumnFilters { get; init; }

    public int PageIndex { get; init; }
}

/// <summary>Context for group header templates.</summary>
public sealed class GridViewGroupContext<TItem>
{
    public string? GroupKey { get; init; }
    public IEnumerable<TItem> Items { get; init; } = [];
    public int Count { get; init; }
}

/// <summary>Event args for row command events (Edit, Delete, Select, custom).</summary>
public sealed class GridViewCommandEventArgs<TItem>
{
    public string CommandName { get; init; } = string.Empty;
    public TItem Item { get; init; } = default!;
    public int RowIndex { get; init; }
}

/// <summary>Event args for RowEditing / RowUpdating events.</summary>
public sealed class GridViewEditEventArgs<TItem>
{
    public TItem Item { get; init; } = default!;
    public int RowIndex { get; init; }
    /// <summary>Set to true to cancel the operation.</summary>
    public bool Cancel { get; set; }
}

/// <summary>Event args for RowDeleting events.</summary>
public sealed class GridViewDeleteEventArgs<TItem>
{
    public TItem Item { get; init; } = default!;
    public int RowIndex { get; init; }
    /// <summary>Set to true to cancel the deletion.</summary>
    public bool Cancel { get; set; }
}

/// <summary>Event args for page change.</summary>
public sealed class GridViewPageEventArgs
{
    public int NewPageIndex { get; init; }
}

/// <summary>Event args for sort change.</summary>
public sealed class GridViewSortEventArgs
{
    public string SortExpression { get; init; } = string.Empty;
    public bool Descending { get; init; }
    public List<GridViewSortInfo> SortColumns { get; init; } = [];
}
