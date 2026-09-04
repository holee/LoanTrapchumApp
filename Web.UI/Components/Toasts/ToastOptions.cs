namespace Web.UI.Components.Toasts;

public sealed class ToastOptions
{
    /// <summary>Position of the toast container.</summary>
    public ToastPosition Position { get; set; } = ToastPosition.TopRight;

    /// <summary>When true, a toast with the same Title+Message+Level won't be shown again while one is still visible.</summary>
    public bool PreventDuplicates { get; set; }

    /// <summary>Show or hide the close button on each toast.</summary>
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>When true, newest toasts appear on top.</summary>
    public bool NewestOnTop { get; set; } = true;

    /// <summary>When true, the progress bar is visible.</summary>
    public bool ShowProgressBar { get; set; } = true;

    /// <summary>When true, hovering pauses the auto-dismiss countdown.</summary>
    public bool CloseOnHover { get; set; } = true;
}
