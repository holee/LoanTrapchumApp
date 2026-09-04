using Application.Features.Notifications.Dto;
using Application.Interfaces;

namespace Application.Features.Notifications.Queries.GetNotification;

public sealed class GetUnreadNotificationQuerySummaryHandler(INotificationService service) : IRequestHandler<GetUnreadNotificationSummaryQuery, NotificationSummaryDto>
{
    public async Task<NotificationSummaryDto> Handle(GetUnreadNotificationSummaryQuery request, CancellationToken cancellationToken = default)
    {
        return await service.GetNotificationSummaryAsync(request.UserId, request.BranchId, request.Take, cancellationToken);
    }
}