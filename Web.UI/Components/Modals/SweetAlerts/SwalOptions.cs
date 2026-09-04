namespace Web.UI.Components.Modals.SweetAlerts;

/// <summary>Configuration options for a SweetAlert-style dialog.</summary>
public sealed class SwalOptions
{
    /// <summary>The title of the dialog. Supports HTML.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>The text body of the dialog.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>Custom HTML body (overrides Text when set).</summary>
    public string? Html { get; set; }

    /// <summary>The icon to display.</summary>
    public SwalIcon Icon { get; set; } = SwalIcon.None;

    /// <summary>Label for the confirm button. Set to null to hide it.</summary>
    public string? ConfirmButtonText { get; set; } = "OK";

    /// <summary>Label for the deny button. Set to null to hide it.</summary>
    public string? DenyButtonText { get; set; }

    /// <summary>Label for the cancel button. Set to null to hide it.</summary>
    public string? CancelButtonText { get; set; }

    /// <summary>Background color for the confirm button.</summary>
    public string ConfirmButtonColor { get; set; } = "#3085d6";

    /// <summary>Background color for the deny button.</summary>
    public string DenyButtonColor { get; set; } = "#dd6b55";

    /// <summary>Background color for the cancel button.</summary>
    public string CancelButtonColor { get; set; } = "#aaa";

    /// <summary>Whether to show a close (×) button in the top-right corner.</summary>
    public bool ShowCloseButton { get; set; } = false;

    /// <summary>Auto-close the dialog after this many milliseconds (0 = disabled).</summary>
    public int TimerMs { get; set; } = 0;

    /// <summary>Show a progress bar when a timer is active.</summary>
    public bool ShowTimerProgressBar { get; set; } = true;

    /// <summary>Whether clicking outside the dialog closes it (dismiss = Backdrop).</summary>
    public bool AllowOutsideClick { get; set; } = false;

    /// <summary>Whether pressing Esc closes the dialog (dismiss = Esc).</summary>
    public bool AllowEscapeKey { get; set; } = true;

    /// <summary>When set, the dialog shows an input field of this type.</summary>
    public SwalInputType? InputType { get; set; }

    /// <summary>Placeholder text for the input field.</summary>
    public string InputPlaceholder { get; set; } = string.Empty;

    /// <summary>Pre-filled value for the input field.</summary>
    public string InputValue { get; set; } = string.Empty;

    /// <summary>Validation message shown when input is required but empty.</summary>
    public string InputValidationMessage { get; set; } = "This field is required.";

    /// <summary>Whether the input is required before confirming.</summary>
    public bool InputRequired { get; set; } = false;

    /// <summary>Custom dialog width (e.g. "32em", "500px").</summary>
    public string Width { get; set; } = "32em";

    /// <summary>Whether to animate the dialog in/out.</summary>
    public bool Animation { get; set; } = true;

    /// <summary>Whether to reverse the button order (cancel left, confirm right by default).</summary>
    public bool ReverseButtons { get; set; } = false;

    /// <summary>Footer HTML text shown below the button bar.</summary>
    public string? Footer { get; set; }
}
