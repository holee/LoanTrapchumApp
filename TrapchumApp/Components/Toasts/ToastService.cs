namespace TrapchumApp.Components.Toasts;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error
}

public enum ToastPosition
{
    TopRight,
    TopLeft,
    TopCenter,
    TopFullWidth,
    BottomRight,
    BottomLeft,
    BottomCenter,
    BottomFullWidth
}

public record ToastMessage(
    string Id,
    string Title,
    string Message,
    ToastLevel Level,
    int DurationMs = 5000
);

public sealed class ToastService(ToastOptions options)
{
    public event Action<ToastMessage>? OnShow;
    public event Action<string>? OnRemove;
    public event Action? OnClearAll;

    /// <summary>Configuration options supplied at registration time.</summary>
    public ToastOptions Options { get; } = options;

    public void ShowInfo(string title, string message, int durationMs = 5000)
        => Show(title, message, ToastLevel.Info, durationMs);

    public void ShowSuccess(string message, string title = "", int durationMs = 5000)
        => Show(title, message, ToastLevel.Success, durationMs);

    public void ShowWarning(string message, string title = "", int durationMs = 5000)
        => Show(title, message, ToastLevel.Warning, durationMs);

    public void ShowError(string message, string title = "", int durationMs = 5000)
        => Show(title, message, ToastLevel.Error, durationMs);

    public void Show(string title, string message, ToastLevel level, int durationMs)
    {
        var toast = new ToastMessage(Guid.NewGuid().ToString(), title, message, level, durationMs);
        OnShow?.Invoke(toast);
    }

    public void Remove(string id) => OnRemove?.Invoke(id);

    public void ClearAll() => OnClearAll?.Invoke();
}
