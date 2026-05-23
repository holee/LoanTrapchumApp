namespace TrapchumApp.Components.Modals.Dialoques;

/// <summary>Configuration options passed to <see cref="ModalService.OpenAsync{TComponent}"/>.</summary>
public sealed class ModalOptions
{
    /// <summary>Dialog size preset. Default is <see cref="ModalSize.Medium"/>.</summary>
    public ModalSize Size { get; set; } = ModalSize.Medium;

    /// <summary>Override the computed width with an explicit CSS value (e.g. "780px", "50vw").
    /// When set, <see cref="Size"/> is ignored for width.</summary>
    public string? CustomWidth { get; set; }

    /// <summary>Override the computed height. Only used for <see cref="ModalSize.Fullscreen"/>
    /// by default; set explicitly to force a fixed height on other sizes.</summary>
    public string? CustomHeight { get; set; }

    /// <summary>Whether the dialog title bar is shown.</summary>
    public bool ShowTitle { get; set; } = true;

    /// <summary>Whether the built-in × close button is shown.</summary>
    public bool ShowClose { get; set; } = true;

    /// <summary>Whether the user can drag/move the dialog.</summary>
    public bool Draggable { get; set; } = true;

    /// <summary>Whether clicking the backdrop closes the dialog.</summary>
    public bool CloseOnBackdrop { get; set; } = false;

    /// <summary>Whether pressing Esc closes the dialog.</summary>
    public bool CloseOnEsc { get; set; } = true;

    /// <summary>Whether to use the full-height style (scrollable content area).</summary>
    public bool Resizable { get; set; } = false;

    /// <summary>Additional inline style appended to the Radzen dialog element.</summary>
    public string? AdditionalStyle { get; set; }
}
