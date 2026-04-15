using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Datatabes;

public partial class BlabTableColumn<TItem> : ComponentBase, IDisposable
{
    [CascadingParameter] public BlabTable<TItem> ParentTable { get; set; } = default!;

    /// <summary>Property name used for sorting and identification.</summary>
    [Parameter] public string? Property { get; set; }

    /// <summary>Column header text.</summary>
    [Parameter] public string Title { get; set; } = string.Empty;

    /// <summary>Whether this column is sortable.</summary>
    [Parameter] public bool Sortable { get; set; } = true;

    /// <summary>CSS width (e.g., "8rem", "120px").</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Text alignment for both header and cells.</summary>
    [Parameter] public string TextAlign { get; set; } = "left";

    /// <summary>Custom cell template. Receives the row TItem as context.</summary>
    [Parameter] public RenderFragment<TItem>? Template { get; set; }

    /// <summary>Custom header template.</summary>
    [Parameter] public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>Whether column is visible.</summary>
    [Parameter] public bool Visible { get; set; } = true;

    /// <summary>CSS class for the column cells.</summary>
    [Parameter] public string? CssClass { get; set; }

    /// <summary>Whether this column supports inline editing.</summary>
    [Parameter] public bool Editable { get; set; }

    /// <summary>
    /// Custom edit template rendered when the cell/row is in edit mode.
    /// Receives the row TItem as context. When null, a default text input is rendered.
    /// </summary>
    [Parameter] public RenderFragment<TItem>? EditTemplate { get; set; }

    protected override void OnInitialized()
    {
        ParentTable?.AddColumn(this);
    }

    public void Dispose()
    {
        ParentTable?.RemoveColumn(this);
    }
}
