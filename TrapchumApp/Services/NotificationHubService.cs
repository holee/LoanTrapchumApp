using Applications.Features.Notifications.Dto;
using Microsoft.AspNetCore.SignalR;
using TrapchumApp.Hubs;

namespace TrapchumApp.Services;

/// <summary>
/// Service for sending real-time notifications via SignalR hub
/// </summary>
public sealed class NotificationHubService(IHubContext<AppNotificationHub> hubContext)
{
    /// <summary>
    /// Sends a notification to a specific user
    /// </summary>
    public async Task SendToUserAsync(string userId, NotificationDto notification)
    {
        await hubContext.Clients
            .Group($"user_{userId}")
            .SendAsync(AppNotificationHub.ReceiveNotification, notification);
    }

    /// <summary>
    /// Sends a notification to all users in a specific branch
    /// </summary>
    public async Task SendToBranchAsync(int branchId, NotificationDto notification)
    {
        await hubContext.Clients
            .Group($"branch_{branchId}")
            .SendAsync(AppNotificationHub.ReceiveNotification, notification);
    }

    /// <summary>
    /// Sends a notification to multiple users
    /// </summary>
    public async Task SendToUsersAsync(IEnumerable<string> userIds, NotificationDto notification)
    {
        var tasks = userIds.Select(userId => SendToUserAsync(userId, notification));
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Broadcasts a notification to all connected clients
    /// </summary>
    public async Task BroadcastAsync(NotificationDto notification)
    {
        await hubContext.Clients.All
            .SendAsync(AppNotificationHub.ReceiveNotification, notification);
    }

    /// <summary>
    /// Sends updated unread count to a specific user
    /// </summary>
    public async Task SendUnreadCountAsync(string userId, int count)
    {
        await hubContext.Clients
            .Group($"user_{userId}")
            .SendAsync(AppNotificationHub.ReceiveNotificationCount, count);
    }

    /// <summary>
    /// Sends a notification to a specific group
    /// </summary>
    public async Task SendToGroupAsync(string groupName, NotificationDto notification)
    {
        await hubContext.Clients
            .Group(groupName)
            .SendAsync(AppNotificationHub.ReceiveNotification, notification);
    }
}
