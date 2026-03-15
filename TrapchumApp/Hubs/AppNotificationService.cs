using Microsoft.AspNetCore.SignalR;

namespace TrapchumApp.Hubs;

public class AppNotificationService : IAppNotificationService
{
    private readonly IHubContext<AppNotificationHub> _hub;

    public AppNotificationService(IHubContext<AppNotificationHub> hub)
    {
        _hub = hub;
    }

    public AppNotification Create(string title, string message, AppNotificationLevel level = AppNotificationLevel.Info)
        => new(Guid.NewGuid().ToString(), title, message, level, DateTime.UtcNow);

    public async Task SendToAllAsync(AppNotification notification)
        => await _hub.Clients.All.SendAsync("ReceiveNotification", notification);

    public async Task SendToUserAsync(string userId, AppNotification notification)
        => await _hub.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", notification);

    public async Task SendToGroupAsync(string group, AppNotification notification)
        => await _hub.Clients.Group(group).SendAsync("ReceiveNotification", notification);
}