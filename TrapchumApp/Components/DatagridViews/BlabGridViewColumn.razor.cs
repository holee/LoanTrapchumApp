namespace TrapchumApp.Components.DatagridViews;

public partial class BlabGridViewColumn<TItem> : ComponentBase, IDisposable
{
    [CascadingParameter] public BlabGridView<TItem> ParentGrid { get; set; } = default!;

    // ── Field type ──────────────────────────────────────────────────
    /// <summary>The type of field this column represents (BoundField, TemplateField, CommandField, etc.).</summary>
    [Parameter] public GridViewFieldType FieldType { get; set; } = GridViewFieldType.BoundField;

    // ── Common properties ───────────────────────────────────────────
    /// <summary>Property name on TItem to bind to (used for BoundField, CheckBoxField, ImageField, HyperLinkField).</summary>
    [Parameter] public string? DataField { get; set; }

    /// <summary>Column header text.</summary>
    [Parameter] public string HeaderText { get; set; } = string.Empty;

    /// <summary>Format string for display (e.g., "{0:N2}", "{0:dd/MM/yyyy}").</summary>
    [Parameter] public string? DataFormatString { get; set; }

    /// <summary>Whether this column is visible.</summary>
    [Parameter] public bool Visible { get; set; } = true;

    /// <summary>CSS width (e.g., "8rem", "120px").</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Text alignment for both header and cells.</summary>
    [Parameter] public string TextAlign { get; set; } = "left";

    /// <summary>CSS class for the column cells.</summary>
    [Parameter] public string? CssClass { get; set; }

    /// <summary>Whether this column is read-only during editing.</summary>
    [Parameter] public bool ReadOnly { get; set; }

    /// <summary>When true, this column is frozen (sticky) on horizontal scroll.</summary>
    [Parameter] public bool Frozen { get; set; }

    // ── Sorting ─────────────────────────────────────────────────────
    /// <summary>Sort expression (property name for sorting). If empty, sorting is disabled for this column.</summary>
    [Parameter] public string? SortExpression { get; set; }

    /// <summary>Whether this column is sortable. Defaults to true if SortExpression or DataField is set.</summary>
    [Parameter] public bool Sortable { get; set; } = true;

    // ── Filtering ───────────────────────────────────────────────────
    /// <summary>Whether this column shows a per-column search filter input.</summary>
    [Parameter] public bool Filterable { get; set; }

    /// <summary>Placeholder text for the per-column filter input.</summary>
    [Parameter] public string FilterPlaceholder { get; set; } = "Filter...";

    // ── Templates (TemplateField) ───────────────────────────────────
    /// <summary>Custom cell template. Receives the row TItem as context.</summary>
    [Parameter] public RenderFragment<TItem>? Template { get; set; }

    /// <summary>Custom header template.</summary>
    [Parameter] public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>Custom edit template rendered when the row is in edit mode.</summary>
    [Parameter] public RenderFragment<TItem>? EditTemplate { get; set; }

    /// <summary>Custom footer template rendered in the tfoot section.</summary>
    [Parameter] public RenderFragment? FooterTemplate { get; set; }

    // ── CommandField properties ──────────────────────────────────────
    /// <summary>Show Edit button in CommandField.</summary>
    [Parameter] public bool ShowEditButton { get; set; }

    /// <summary>Show Delete button in CommandField.</summary>
    [Parameter] public bool ShowDeleteButton { get; set; }

    /// <summary>Show Select button in CommandField.</summary>
    [Parameter] public bool ShowSelectButton { get; set; }

    /// <summary>Text for Edit button.</summary>
    [Parameter] public string EditText { get; set; } = "Edit";

    /// <summary>Text for Update button (shown during edit mode).</summary>
    [Parameter] public string UpdateText { get; set; } = "Update";

    /// <summary>Text for Cancel button (shown during edit mode).</summary>
    [Parameter] public string CancelText { get; set; } = "Cancel";

    /// <summary>Text for Delete button.</summary>
    [Parameter] public string DeleteText { get; set; } = "Delete";

    /// <summary>Text for Select button.</summary>
    [Parameter] public string SelectText { get; set; } = "Select";

    // ── ButtonField properties ──────────────────────────────────────
    /// <summary>Display text for ButtonField.</summary>
    [Parameter] public string? ButtonText { get; set; }

    /// <summary>Command name fired when ButtonField is clicked.</summary>
    [Parameter] public string? CommandName { get; set; }

    /// <summary>CSS class for the button.</summary>
    [Parameter] public string? ButtonCssClass { get; set; }

    // ── HyperLinkField properties ───────────────────────────────────
    /// <summary>Static navigate URL for HyperLinkField. Use DataNavigateUrlField for dynamic URLs.</summary>
    [Parameter] public string? NavigateUrl { get; set; }

    /// <summary>Property name on TItem that provides the URL for HyperLinkField.</summary>
    [Parameter] public string? DataNavigateUrlField { get; set; }

    /// <summary>Property name on TItem that provides the display text for HyperLinkField.</summary>
    [Parameter] public string? DataTextField { get; set; }

    /// <summary>Link target attribute (e.g., "_blank").</summary>
    [Parameter] public string? Target { get; set; }

    // ── ImageField properties ───────────────────────────────────────
    /// <summary>Property name on TItem that provides the image URL for ImageField.</summary>
    [Parameter] public string? DataImageUrlField { get; set; }

    /// <summary>Property name on TItem that provides the alt text for ImageField.</summary>
    [Parameter] public string? DataAlternateTextField { get; set; }

    /// <summary>CSS class for the image element.</summary>
    [Parameter] public string? ImageCssClass { get; set; }

    /// <summary>Width for the image (e.g., "40px").</summary>
    [Parameter] public string ImageWidth { get; set; } = "40px";

    /// <summary>Height for the image (e.g., "40px").</summary>
    [Parameter] public string ImageHeight { get; set; } = "40px";

    // ── Computed helpers ────────────────────────────────────────────
    /// <summary>The effective sort property (SortExpression ?? DataField).</summary>
    internal string? EffectiveSortProperty => SortExpression ?? DataField;

    /// <summary>Whether this is a command-type column (CommandField or ButtonField).</summary>
    internal bool IsCommandType => FieldType is GridViewFieldType.CommandField or GridViewFieldType.ButtonField;

    // ── Lifecycle ───────────────────────────────────────────────────
    protected override void OnInitialized()
    {
        ParentGrid?.AddColumn(this);
    }

    public void Dispose()
    {
        ParentGrid?.RemoveColumn(this);
    }
}
