using Application.Features.Notifications.Dto;

namespace Application.Features.Notifications.Queries.GetUnreadNotifications;

public record GetUnreadNotificationsQuery(string? UserId, int? BranchId, QueryParam Param) : IRequest<Pager<NotificationDto>>;