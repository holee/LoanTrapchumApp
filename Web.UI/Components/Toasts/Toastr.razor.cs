using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Components.Toasts;

public partial class Toastr : IDisposable
{
    [Inject] private ToastService ToastService { get; set; } = default!;

    private readonly List<ToastMessage> _toasts = [];
    private readonly HashSet<string> _removing = [];
    private readonly ConcurrentDictionary<string, ToastTimerState> _timers = new();
    private readonly object _lock = new();

    protected override void OnInitialized()
    {
        ToastService.OnShow += HandleShow;
        ToastService.OnRemove += HandleRemove;
        ToastService.OnClearAll += HandleClearAll;
    }

    // ── Position CSS class ──────────────────────────────────────────
    private string PositionClass => ToastService.Options.Position switch
    {
        ToastPosition.TopRight       => "toast-top-right",
        ToastPosition.TopLeft        => "toast-top-left",
        ToastPosition.TopCenter      => "toast-top-center",
        ToastPosition.TopFullWidth   => "toast-top-full-width",
        ToastPosition.BottomRight    => "toast-bottom-right",
        ToastPosition.BottomLeft     => "toast-bottom-left",
        ToastPosition.BottomCenter   => "toast-bottom-center",
        ToastPosition.BottomFullWidth => "toast-bottom-full-width",
        _ => "toast-top-right"
    };

    // ── Show ────────────────────────────────────────────────────────
    private async void HandleShow(ToastMessage toast)
    {
        lock (_lock)
        {
            if (ToastService.Options.PreventDuplicates &&
                _toasts.Exists(t => t.Title == toast.Title &&
                                    t.Message == toast.Message &&
                                    t.Level == toast.Level))
                return;

            if (ToastService.Options.NewestOnTop)
                _toasts.Insert(0, toast);
            else
                _toasts.Add(toast);
        }

        await InvokeAsync(StateHasChanged);

        if (toast.DurationMs > 0)
        {
            StartTimer(toast.Id, toast.DurationMs);
        }
    }

    // ── Remove / ClearAll ───────────────────────────────────────────
    private async void HandleRemove(string id) => await AnimateAndRemoveAsync(id);

    private async void HandleClearAll()
    {
        List<string> ids;
        lock (_lock)
        {
            ids = _toasts.Select(t => t.Id).ToList();
        }

        foreach (var id in ids)
            await AnimateAndRemoveAsync(id);
    }

    private void Dismiss(string id) => _ = AnimateAndRemoveAsync(id);

    private async Task AnimateAndRemoveAsync(string id)
    {
        lock (_lock)
        {
            if (!_toasts.Exists(t => t.Id == id))
                return;

            _removing.Add(id);
        }

        CancelTimer(id);
        await InvokeAsync(StateHasChanged);
        await Task.Delay(300);

        lock (_lock)
        {
            _toasts.RemoveAll(t => t.Id == id);
            _removing.Remove(id);
        }

        await InvokeAsync(StateHasChanged);
    }

    private bool IsRemoving(string id)
    {
        lock (_lock) { return _removing.Contains(id); }
    }

    // ── Timer with hover-pause ──────────────────────────────────────
    private void StartTimer(string id, int durationMs)
    {
        var cts = new CancellationTokenSource();
        var state = new ToastTimerState(durationMs, durationMs, cts);
        _timers[id] = state;
        _ = RunTimerAsync(id, state);
    }

    private async Task RunTimerAsync(string id, ToastTimerState state)
    {
        try
        {
            while (state.RemainingMs > 0)
            {
                const int tick = 50;
                await Task.Delay(tick, state.Cts.Token);

                if (!state.Paused)
                    state.RemainingMs -= tick;
            }

            await AnimateAndRemoveAsync(id);
        }
        catch (TaskCanceledException) { }
    }

    private void CancelTimer(string id)
    {
        if (_timers.TryRemove(id, out var state))
            state.Cts.Cancel();
    }

    // ── Mouse enter / leave (hover-pause) ───────────────────────────
    private async Task OnMouseEnter(string id)
    {
        if (!ToastService.Options.CloseOnHover) return;

        if (_timers.TryGetValue(id, out var state))
            state.Paused = true;

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnMouseLeave(string id)
    {
        if (!ToastService.Options.CloseOnHover) return;

        if (_timers.TryGetValue(id, out var state))
            state.Paused = false;

        await InvokeAsync(StateHasChanged);
    }

    private bool IsPaused(string id)
    {
        return _timers.TryGetValue(id, out var s) && s.Paused;
    }

    // ── Dispose ─────────────────────────────────────────────────────
    public void Dispose()
    {
        ToastService.OnShow -= HandleShow;
        ToastService.OnRemove -= HandleRemove;
        ToastService.OnClearAll -= HandleClearAll;

        foreach (var kvp in _timers)
            kvp.Value.Cts.Cancel();

        _timers.Clear();
    }

    // ── Internal timer state ────────────────────────────────────────
    private sealed class ToastTimerState(int totalMs, int remainingMs, CancellationTokenSource cts)
    {
        public int TotalMs { get; } = totalMs;
        public int RemainingMs { get; set; } = remainingMs;
        public bool Paused { get; set; }
        public CancellationTokenSource Cts { get; } = cts;
    }
}
