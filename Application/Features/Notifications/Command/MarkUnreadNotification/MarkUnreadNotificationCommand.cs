using Application.Features.Notifications.Dto;
using ErrorOr;

namespace Application.Features.Notifications.Queries.GetNotification;

public record MarkUnreadNotificationCommand(Guid Id, int? BranchId) : IRequest<ErrorOr<NotificationDto>>;
