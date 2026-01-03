using Applications.Features.Notifications.Dto;
using Applications.Interfaces;

namespace Applications.Features.Notifications.Queries.GetNotification;

public sealed class GetNotificationAsPagingQueryHandler(INotificationService service) : IRequestHandler<GetNotificationAsPagingQuery, Pager<NotificationDto>>
{
    public async Task<Pager<NotificationDto>> Handle(GetNotificationAsPagingQuery request, CancellationToken cancellationToken = default)
    {
        return await service.GetLatestAsync(request.UserId, request.BranchId, request.state, request.Param, cancellationToken);
    }
}