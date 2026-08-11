namespace TrapchumApp.Components.Select2;

/// <summary>
/// Localization strings for the BlabSelect2 component.
/// Mirrors Select2.js language/i18n support.
/// </summary>
public class Select2Language
{
    /// <summary>Message shown when no results match the search.</summary>
    public string NoResults { get; set; } = "No results found";

    /// <summary>Message shown while an async search is in progress.</summary>
    public string Searching { get; set; } = "Searching…";

    /// <summary>
    /// Format string for the minimum-input-length hint.
    /// Use {0} for the remaining character count.
    /// </summary>
    public string InputTooShort { get; set; } = "Please enter {0} or more characters";

    /// <summary>
    /// Format string when <see cref="BlabSelect2{TItem,TValue}.MaximumInputLength"/> is exceeded.
    /// Use {0} for the number of characters to delete.
    /// </summary>
    public string InputTooLong { get; set; } = "Please delete {0} character(s)";

    /// <summary>Message shown when loading additional pages of results.</summary>
    public string LoadingMore { get; set; } = "Loading more results…";

    /// <summary>
    /// Callback that returns a message when the maximum number of
    /// selections has been reached. Receives the limit as an argument.
    /// Falls back to a built-in default if null.
    /// </summary>
    public Func<int, string>? MaximumSelected { get; set; }
}
