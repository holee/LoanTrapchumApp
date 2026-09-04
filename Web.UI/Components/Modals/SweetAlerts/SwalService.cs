using Radzen;

namespace Web.UI.Components.Modals.SweetAlerts;

/// <summary>
/// SweetAlert-style dialog service. Inject and call <see cref="FireAsync"/> or the
/// shortcut helpers (SuccessAsync, ConfirmAsync, …) from any Blazor component.
/// </summary>
public sealed class SwalService(DialogService dialogService)
{
    // ── Core ──────────────────────────────────────────────────────────────

    /// <summary>Display a fully customised SweetAlert-style dialog.</summary>
    public async Task<SwalResult> FireAsync(SwalOptions options)
    {
        var parameters = new Dictionary<string, object?>
        {
            { nameof(SweatAlert.Options), options }
        };

        var dialogOptions = new DialogOptions
        {
            ShowTitle  = false,
            ShowClose  = false,
            Draggable  = false,
            CloseDialogOnOverlayClick = options.AllowOutsideClick,
            CloseDialogOnEsc          = options.AllowEscapeKey,
            Width  = "auto",
            Style  = "padding:0;border:none;background:transparent;box-shadow:none;overflow:visible;"
        };

        var raw = await dialogService.OpenAsync<SweatAlert>(string.Empty, parameters, dialogOptions);

        if (raw is SwalResult result)
            return result;

        // Dialog closed by overlay click or Esc (Radzen returns null)
        return SwalResult.Dismissed(SwalDismissReason.Backdrop);
    }

    // ── Shortcut helpers ──────────────────────────────────────────────────

    /// <summary>Simple alert with a success icon.</summary>
    public Task<SwalResult> SuccessAsync(string title, string text = "", string? footer = null)
        => FireAsync(new SwalOptions
        {
            Title  = title,
            Text   = text,
            Icon   = SwalIcon.Success,
            Footer = footer
        });

    /// <summary>Simple alert with an error icon.</summary>
    public Task<SwalResult> ErrorAsync(string title, string text = "", string? footer = null)
        => FireAsync(new SwalOptions
        {
            Title  = title,
            Text   = text,
            Icon   = SwalIcon.Error,
            Footer = footer
        });

    /// <summary>Simple alert with a warning icon.</summary>
    public Task<SwalResult> WarningAsync(string title, string text = "", string? footer = null)
        => FireAsync(new SwalOptions
        {
            Title  = title,
            Text   = text,
            Icon   = SwalIcon.Warning,
            Footer = footer
        });

    /// <summary>Simple alert with an info icon.</summary>
    public Task<SwalResult> InfoAsync(string title, string text = "", string? footer = null)
        => FireAsync(new SwalOptions
        {
            Title  = title,
            Text   = text,
            Icon   = SwalIcon.Info,
            Footer = footer
        });

    /// <summary>Simple alert with a question icon.</summary>
    public Task<SwalResult> QuestionAsync(string title, string text = "")
        => FireAsync(new SwalOptions
        {
            Title = title,
            Text  = text,
            Icon  = SwalIcon.Question
        });

    /// <summary>Confirm/cancel dialog. Returns true when the user confirmed.</summary>
    public async Task<bool> ConfirmAsync(
        string title,
        string text             = "",
        SwalIcon icon           = SwalIcon.Question,
        string confirmText      = "Yes",
        string cancelText       = "Cancel",
        string confirmColor     = "#3085d6",
        string cancelColor      = "#aaa")
    {
        var result = await FireAsync(new SwalOptions
        {
            Title               = title,
            Text                = text,
            Icon                = icon,
            ConfirmButtonText   = confirmText,
            CancelButtonText    = cancelText,
            ConfirmButtonColor  = confirmColor,
            CancelButtonColor   = cancelColor
        });
        return result.IsConfirmed;
    }

    /// <summary>Three-button confirm/deny/cancel dialog.</summary>
    public Task<SwalResult> ConfirmDenyAsync(
        string title,
        string text            = "",
        SwalIcon icon          = SwalIcon.Question,
        string confirmText     = "Yes",
        string denyText        = "No",
        string cancelText      = "Cancel")
        => FireAsync(new SwalOptions
        {
            Title             = title,
            Text              = text,
            Icon              = icon,
            ConfirmButtonText = confirmText,
            DenyButtonText    = denyText,
            CancelButtonText  = cancelText
        });

    /// <summary>Dialog with a text input. Returns the entered value or null when cancelled.</summary>
    public async Task<string?> InputAsync(
        string title,
        string text                 = "",
        SwalInputType inputType     = SwalInputType.Text,
        string placeholder          = "",
        string confirmText          = "OK",
        string cancelText           = "Cancel",
        bool required               = false,
        string validationMessage    = "This field is required.")
    {
        var result = await FireAsync(new SwalOptions
        {
            Title                  = title,
            Text                   = text,
            Icon                   = SwalIcon.Question,
            InputType              = inputType,
            InputPlaceholder       = placeholder,
            ConfirmButtonText      = confirmText,
            CancelButtonText       = cancelText,
            InputRequired          = required,
            InputValidationMessage = validationMessage
        });

        return result.IsConfirmed ? result.Value ?? string.Empty : null;
    }

    /// <summary>Auto-closing alert that dismisses after <paramref name="timerMs"/> milliseconds.</summary>
    public Task<SwalResult> TimedAsync(
        string title,
        string text    = "",
        SwalIcon icon  = SwalIcon.Info,
        int timerMs    = 3000)
        => FireAsync(new SwalOptions
        {
            Title                 = title,
            Text                  = text,
            Icon                  = icon,
            TimerMs               = timerMs,
            ShowTimerProgressBar  = true,
            ConfirmButtonText     = null,
            ShowCloseButton       = false,
            AllowOutsideClick     = false,
            AllowEscapeKey        = false
        });
}
