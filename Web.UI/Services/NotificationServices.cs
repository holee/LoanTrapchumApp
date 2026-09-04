using Applications.Features.Notifications.Dto;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Web.UI.Services;

public sealed class NotificationServices : IAsyncDisposable
{
    public const string HubUrl = "/notificationHub";
    public const string ReceiveNotification = "ReceiveNotification";
    public const string ReceiveNotificationCount = "ReceiveNotificationCount";

    public event Action? OnChange;

    private int _unreadCount;
    private List<NotificationDto> _notifications = [];
    private HubConnection? _hubConnection;
    private string? _userId;
    private int? _branchId;

    public int UnReadCount
    {
        get => _unreadCount;
        set
        {
            if (_unreadCount == value) return;
            _unreadCount = value;
            NotifyStateChanged();
        }
    }

    public List<NotificationDto> Notifications
    {
        get => _notifications;
        set
        {
            _notifications = value;
            NotifyStateChanged();
        }
    }

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
    public string? UserId => _userId;
    public int? BranchId => _branchId;

    public async Task StartConnectionAsync(string baseUrl, string? userId, int? branchId)
    {
        if (_hubConnection is not null)
        {
            await StopConnectionAsync();
        }

        _userId = userId;
        _branchId = branchId;

        var hubUrl = $"{baseUrl}{HubUrl}?userId={userId}&branchId={branchId}";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.Transports = HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
                options.SkipNegotiation = false;
            })
            .WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)])
            .Build();

        RegisterHubHandlers();

        try
        {
            await _hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
        }
    }

    private void RegisterHubHandlers()
    {
        if (_hubConnection is null) return;

        _hubConnection.On<NotificationDto>(ReceiveNotification, notification =>
        {
            AddNotification(notification);
        });

        _hubConnection.On<int>(ReceiveNotificationCount, count =>
        {
            UnReadCount = count;
        });

        _hubConnection.Reconnecting += error =>
        {
            Console.WriteLine($"SignalR reconnecting: {error?.Message}");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += connectionId =>
        {
            Console.WriteLine($"SignalR reconnected: {connectionId}");
            NotifyStateChanged();
            return Task.CompletedTask;
        };

        _hubConnection.Closed += error =>
        {
            Console.WriteLine($"SignalR connection closed: {error?.Message}");
            return Task.CompletedTask;
        };
    }

    public async Task StopConnectionAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public void AddNotification(NotificationDto notification)
    {
        _notifications.Insert(0, notification);
        if (!notification.IsRead)
        {
            _unreadCount++;
        }
        NotifyStateChanged();
    }

    public void DecreaseUnread()
    {
        if (_unreadCount > 0)
        {
            _unreadCount--;
            NotifyStateChanged();
        }
    }

    public void DecreaseUnread(NotificationDto notification)
    {
        if (_notifications.Remove(notification))
        {
            _unreadCount = Math.Max(0, _unreadCount - 1);
            NotifyStateChanged();
        }
    }

    public void MarkAsRead(NotificationDto notification)
    {
        var existing = _notifications.FirstOrDefault(n => n.Id == notification.Id);
        if (existing is not null && !existing.IsRead)
        {
            existing.IsRead = true;
            _unreadCount = Math.Max(0, _unreadCount - 1);
            _notifications.Remove(existing);
            NotifyStateChanged();
        }
    }

    public void Clear()
    {
        _notifications.Clear();
        _unreadCount = 0;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async ValueTask DisposeAsync()
    {
        await StopConnectionAsync();
    }
}
