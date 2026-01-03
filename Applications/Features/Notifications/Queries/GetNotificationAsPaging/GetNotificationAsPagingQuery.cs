using Applications.Features.Notifications.Dto;
using Domains.Enumerations;

namespace Applications.Features.Notifications.Queries.GetNotification;

public record GetNotificationAsPagingQuery(string? UserId, int? BranchId,NotificationStatus state, QueryParam Param) : IRequest<Pager<NotificationDto>>;
