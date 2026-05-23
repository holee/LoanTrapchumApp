using Radzen;

namespace TrapchumApp.Components.Modals.Dialoques;

/// <summary>
/// Wrapper around Radzen <see cref="DialogService"/> that adds size presets
/// (Small, Medium, Large, Fullscreen) and a clean API for opening typed dialogs.
/// </summary>
public sealed class ModalService(DialogService dialogService)
{
    // ── Width / height helpers ────────────────────────────────────────────

    private static string ResolveWidth(ModalOptions opts) =>
        opts.CustomWidth ?? opts.Size switch
        {
            ModalSize.Small      => "400px",
            ModalSize.Medium     => "640px",
            ModalSize.Large      => "960px",
            ModalSize.Fullscreen => "100vw",
            _                    => "640px"
        };

    private static string? ResolveHeight(ModalOptions opts)
    {
        if (opts.CustomHeight is not null) return opts.CustomHeight;
        return opts.Size == ModalSize.Fullscreen ? "100vh" : null;
    }

    private static string BuildStyle(ModalOptions opts)
    {
        var height = ResolveHeight(opts);
        var parts  = new List<string>();

        if (opts.Size == ModalSize.Fullscreen)
        {
            parts.Add("border-radius:0");
            parts.Add("margin:0");
        }

        if (height is not null)
        {
            parts.Add($"height:{height}");
            parts.Add("max-height:100vh");
        }

        if (opts.Resizable)
            parts.Add("resize:both;overflow:auto");

        if (!string.IsNullOrWhiteSpace(opts.AdditionalStyle))
            parts.Add(opts.AdditionalStyle!);

        return string.Join(";", parts);
    }

    private DialogOptions ToDialogOptions(string title, ModalOptions opts) => new()
    {
        ShowTitle                 = opts.ShowTitle,
        ShowClose                 = opts.ShowClose,
        Draggable                 = opts.Draggable && opts.Size != ModalSize.Fullscreen,
        CloseDialogOnOverlayClick = opts.CloseOnBackdrop,
        CloseDialogOnEsc          = opts.CloseOnEsc,
        Width                     = ResolveWidth(opts),
        Style                     = BuildStyle(opts)
    };

    // ── Core API ──────────────────────────────────────────────────────────

    /// <summary>Opens <typeparamref name="TComponent"/> in a modal with the given size and parameters.
    /// Returns whatever value the component passes to <c>DialogService.Close(…)</c>, or <c>null</c>.</summary>
    public Task<dynamic?> OpenAsync<TComponent>(
        string title,
        Dictionary<string, object?>? parameters = null,
        ModalOptions? options = null)
        where TComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        options ??= new ModalOptions();
        return dialogService.OpenAsync<TComponent>(title, parameters, ToDialogOptions(title, options));
    }

    // ── Size shortcuts ────────────────────────────────────────────────────

    /// <summary>Opens a <b>small</b> (400 px) modal.</summary>
    public Task<dynamic?> OpenSmallAsync<TComponent>(
        string title,
        Dictionary<string, object?>? parameters = null,
        Action<ModalOptions>? configure = null)
        where TComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        var opts = new ModalOptions { Size = ModalSize.Small };
        configure?.Invoke(opts);
        return OpenAsync<TComponent>(title, parameters, opts);
    }

    /// <summary>Opens a <b>medium</b> (640 px) modal – the default size.</summary>
    public Task<dynamic?> OpenMediumAsync<TComponent>(
        string title,
        Dictionary<string, object?>? parameters = null,
        Action<ModalOptions>? configure = null)
        where TComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        var opts = new ModalOptions { Size = ModalSize.Medium };
        configure?.Invoke(opts);
        return OpenAsync<TComponent>(title, parameters, opts);
    }

    /// <summary>Opens a <b>large</b> (960 px) modal.</summary>
    public Task<dynamic?> OpenLargeAsync<TComponent>(
        string title,
        Dictionary<string, object?>? parameters = null,
        Action<ModalOptions>? configure = null)
        where TComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        var opts = new ModalOptions { Size = ModalSize.Large };
        configure?.Invoke(opts);
        return OpenAsync<TComponent>(title, parameters, opts);
    }

    /// <summary>Opens a <b>fullscreen</b> (100 vw × 100 vh) modal.</summary>
    public Task<dynamic?> OpenFullscreenAsync<TComponent>(
        string title,
        Dictionary<string, object?>? parameters = null,
        Action<ModalOptions>? configure = null)
        where TComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        var opts = new ModalOptions { Size = ModalSize.Fullscreen, ShowClose = true, Draggable = false };
        configure?.Invoke(opts);
        return OpenAsync<TComponent>(title, parameters, opts);
    }

    /// <summary>Closes the top-most open dialog, optionally passing a return value.</summary>
    public void Close(object? result = null) => dialogService.Close(result);
}
