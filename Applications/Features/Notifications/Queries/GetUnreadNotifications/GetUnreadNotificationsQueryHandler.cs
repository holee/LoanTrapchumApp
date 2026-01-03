using Applications.Features.Notifications.Dto;
using Applications.Interfaces;

namespace Applications.Features.Notifications.Queries.GetUnreadNotifications;

public sealed class GetUnreadNotificationsQueryHandler(INotificationService service) : IRequestHandler<GetUnreadNotificationsQuery, Pager<NotificationDto>>
{
    public async Task<Pager<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken = default)
    {
        return await service.GetLatestAsync(request.UserId, request.BranchId, request.Param, cancellationToken);
    }
}