using Application.Features.Notifications.Dto;
using Application.Interfaces;

namespace Application.Features.Notifications.Queries.GetNotification;

public sealed class GetNotificationAsPagingQueryHandler(INotificationService service) : IRequestHandler<GetNotificationAsPagingQuery, Pager<NotificationDto>>
{
    public async Task<Pager<NotificationDto>> Handle(GetNotificationAsPagingQuery request, CancellationToken cancellationToken = default)
    {
        return await service.GetLatestAsync(request.UserId, request.BranchId, request.state, request.Param, cancellationToken);
    }
}