namespace TrapchumApp.Components.Messageboxes;

/// <summary>
/// Optional styling/behavior for <see cref="MessageBoxService"/>.
/// </summary>
public sealed class MessageBoxOptions
{
    /// <summary>
    /// Per-button color preset keyed by <see cref="MsgBoxResult"/>.
    /// Buttons not listed keep the default gray style.
    /// </summary>
    public Dictionary<MsgBoxResult, MessageBoxButtonColor> ButtonColors { get; set; } = [];

    /// <summary>Allow user to drag/move the dialog. Default <c>true</c>.</summary>
    public bool Draggable { get; set; } = true;
}

internal static class ButtonColorHelper
{
    internal static string ToCss(this MessageBoxButtonColor color) => color switch
    {
        MessageBoxButtonColor.Primary => "background:#1e88e5;color:#fff;border-color:#1976d2;",
        MessageBoxButtonColor.Success => "background:#43a047;color:#fff;border-color:#388e3c;",
        MessageBoxButtonColor.Danger  => "background:#e53935;color:#fff;border-color:#d32f2f;",
        MessageBoxButtonColor.Warning => "background:#fb8c00;color:#fff;border-color:#f57c00;",
        MessageBoxButtonColor.Info    => "background:#0288d1;color:#fff;border-color:#0277bd;",
        MessageBoxButtonColor.Dark    => "background:#424242;color:#fff;border-color:#333;",
        _                             => string.Empty
    };
}
