namespace Applications.Features.Notifications.Queries.GetNotification;

public record GetUnreadNotificationQuery(string? UserId, int? BranchId) : IRequest<int>;
