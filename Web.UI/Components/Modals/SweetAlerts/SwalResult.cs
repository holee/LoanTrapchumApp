namespace Web.UI.Components.Modals.SweetAlerts;

/// <summary>The result returned by <see cref="SwalService.FireAsync"/>.</summary>
public sealed class SwalResult
{
    /// <summary>True when the user clicked the Confirm button.</summary>
    public bool IsConfirmed { get; init; }

    /// <summary>True when the user clicked the Deny button.</summary>
    public bool IsDenied { get; init; }

    /// <summary>True when the dialog was dismissed (cancel, backdrop, Esc, timer).</summary>
    public bool IsDismissed => !IsConfirmed && !IsDenied;

    /// <summary>Reason the dialog was dismissed; null when confirmed or denied.</summary>
    public SwalDismissReason? DismissReason { get; init; }

    /// <summary>The value entered by the user in the input field (if any).</summary>
    public string? Value { get; init; }

    // ── Static factories ──────────────────────────────────────────────────

    public static SwalResult Confirmed(string? value = null) =>
        new() { IsConfirmed = true, Value = value };

    public static SwalResult Denied() =>
        new() { IsDenied = true };

    public static SwalResult Dismissed(SwalDismissReason reason) =>
        new() { DismissReason = reason };
}
