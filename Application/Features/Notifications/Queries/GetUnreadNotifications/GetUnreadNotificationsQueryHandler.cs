using Application.Features.Notifications.Dto;
using Application.Interfaces;

namespace Application.Features.Notifications.Queries.GetUnreadNotifications;

public sealed class GetUnreadNotificationsQueryHandler(INotificationService service) : IRequestHandler<GetUnreadNotificationsQuery, Pager<NotificationDto>>
{
    public async Task<Pager<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken = default)
    {
        return await service.GetLatestAsync(request.UserId, request.BranchId, request.Param, cancellationToken);
    }
}