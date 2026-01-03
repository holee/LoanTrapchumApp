using Applications.Features.Notifications.Dto;

namespace Applications.Features.Notifications.Queries.GetNotification;

public record GetUnreadNotificationSummaryQuery(string? UserId, int? BranchId, int Take = 10) : IRequest<NotificationSummaryDto>;
