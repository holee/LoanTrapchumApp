using Applications.Features.Notifications.Dto;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Notifications.Queries.GetNotification;

public sealed class MarkUnreadNotificationCommandHandler(INotificationService service) 
    : IRequestHandler<MarkUnreadNotificationCommand, ErrorOr<NotificationDto>>
{
    public async Task<ErrorOr<NotificationDto>> Handle(MarkUnreadNotificationCommand request, CancellationToken cancellationToken = default)
    {
        var result = await service.MarkAsReadAsync(request.Id, request.BranchId, cancellationToken);

        if (result is null)
        {
            return Error.NotFound(
                code: "Notification.NotFound",
                description: $"Notification with Id '{request.Id}' was not found.");
        }
        return result;
    }
}


