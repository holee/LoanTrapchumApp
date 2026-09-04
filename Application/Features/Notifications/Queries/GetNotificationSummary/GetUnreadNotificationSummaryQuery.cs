using Application.Features.Notifications.Dto;

namespace Application.Features.Notifications.Queries.GetNotification;

public record GetUnreadNotificationSummaryQuery(string? UserId, int? BranchId, int Take = 10) : IRequest<NotificationSummaryDto>;
