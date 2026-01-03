using Applications.Interfaces;

namespace Applications.Features.Notifications.Queries.GetNotification;

public sealed class GetUnreadNotificationQueryHandler(INotificationService service) : IRequestHandler<GetUnreadNotificationQuery, int>
{
    public async Task<int> Handle(GetUnreadNotificationQuery request, CancellationToken cancellationToken = default)
    {
        return await service.GetUnreadCountAsync(request.UserId, cancellationToken);
    }
}
