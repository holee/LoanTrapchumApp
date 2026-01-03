using Applications.Features.Notifications.Dto;

namespace Applications.Features.Notifications.Queries.GetNotification;

public record GetNotificationQuery(string? UserId, int? BranchId, int Take = 10) : IRequest<List<NotificationDto>>;
