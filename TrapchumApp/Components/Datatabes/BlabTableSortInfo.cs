namespace TrapchumApp.Components.Datatabes;

/// <summary>Represents a single sort column in multi-sort mode.</summary>
public sealed class BlabTableSortInfo
{
    public string Column { get; init; } = string.Empty;
    public bool Descending { get; init; }
}

/// <summary>Selection mode for BlabTable rows.</summary>
public enum BlabTableSelectionMode
{
    /// <summary>No row selection.</summary>
    None,
    /// <summary>Single row selection (radio-style).</summary>
    Single,
    /// <summary>Multiple row selection with checkboxes.</summary>
    Multiple
}
