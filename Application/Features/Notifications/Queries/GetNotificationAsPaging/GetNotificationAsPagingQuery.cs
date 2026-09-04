using Application.Features.Notifications.Dto;
using Domains.Enumerations;

namespace Application.Features.Notifications.Queries.GetNotification;

public record GetNotificationAsPagingQuery(string? UserId, int? BranchId,NotificationStatus state, QueryParam Param) : IRequest<Pager<NotificationDto>>;
