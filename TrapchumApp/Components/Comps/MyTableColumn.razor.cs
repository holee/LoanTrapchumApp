namespace TrapchumApp.Components.Comps;

public partial class MyTableColumn<TItem> : ComponentBase, IDisposable
{
    [CascadingParameter] public MyTable<TItem> ParentTable { get; set; } = default!;

    /// <summary>Column header text.</summary>
    [Parameter] public string Title { get; set; } = string.Empty;

    /// <summary>CSS width (e.g., "8rem", "120px").</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Text alignment for header and cells.</summary>
    [Parameter] public TableTextAlign TextAlign { get; set; } = TableTextAlign.Left;

    /// <summary>Custom cell template. Receives the row TItem as context.</summary>
    [Parameter] public RenderFragment<TItem>? Template { get; set; }

    /// <summary>Whether column is visible.</summary>
    [Parameter] public bool Visible { get; set; } = true;

    /// <summary>CSS class applied to each cell in this column.</summary>
    [Parameter] public string? CssClass { get; set; }

    protected override void OnInitialized()
    {
        ParentTable?.AddColumn(this);
    }

    public void Dispose()
    {
        ParentTable?.RemoveColumn(this);
    }
}
