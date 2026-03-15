namespace TrapchumApp.Hubs;

public interface IAppNotificationService
{
    Task SendToAllAsync(AppNotification notification);
    Task SendToUserAsync(string userId, AppNotification notification);
    Task SendToGroupAsync(string group, AppNotification notification);
    AppNotification Create(string title, string message, AppNotificationLevel level = AppNotificationLevel.Info);
}
