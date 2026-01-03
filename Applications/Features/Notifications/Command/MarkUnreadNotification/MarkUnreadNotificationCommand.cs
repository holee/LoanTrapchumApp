using Applications.Features.Notifications.Dto;
using ErrorOr;

namespace Applications.Features.Notifications.Queries.GetNotification;

public record MarkUnreadNotificationCommand(Guid Id, int? BranchId) : IRequest<ErrorOr<NotificationDto>>;
