namespace TrapchumApp.Hubs;
public record AppNotification(
    string Id,
    string Title,
    string Message,
    AppNotificationLevel Level,
    DateTime CreatedAt
);
