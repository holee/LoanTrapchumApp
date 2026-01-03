using Applications.Features.Notifications.Dto;
using Domains.Models.Identity;
using Microsoft.AspNetCore.SignalR;

namespace TrapchumApp.Hubs;

public class NotificationHub : Hub
{
    public const string HubUrl = "/notificationHub";
    public const string ReceiveNotification = "ReceiveNotification";
    public const string ReceiveNotificationCount = "ReceiveNotificationCount";

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString() ?? string.Empty;

        var branchId = Context.GetHttpContext()?.Request.Query["branchId"].ToString() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Add to user-specific group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        if (!string.IsNullOrWhiteSpace(branchId))
        {
            // Add to branch-specific group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"branch_{branchId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

        var branchId = Context.GetHttpContext()?.Request.Query["branchId"].ToString();


        if (!string.IsNullOrWhiteSpace(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        if (!string.IsNullOrWhiteSpace(branchId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"branch_{branchId}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Joins a specific notification group
    /// </summary>
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Leaves a specific notification group
    /// </summary>
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}

