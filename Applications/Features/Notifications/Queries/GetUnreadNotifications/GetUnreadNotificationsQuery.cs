using Applications.Features.Notifications.Dto;

namespace Applications.Features.Notifications.Queries.GetUnreadNotifications;

public record GetUnreadNotificationsQuery(string? UserId, int? BranchId, QueryParam Param) : IRequest<Pager<NotificationDto>>;