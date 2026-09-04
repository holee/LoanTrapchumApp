using Application.Features.Notifications.Dto;
using Application.Interfaces;

namespace Application.Features.Notifications.Queries.GetNotification;

//public sealed class GetNotificationQueryHandler(INotificationService service) : IRequestHandler<GetNotificationQuery, List<NotificationDto>>
//{
//    public async Task<List<NotificationDto>> Handle(GetNotificationQuery request, CancellationToken cancellationToken = default)
//    {
//        return await service.GetLatestAsync(request.UserId, request.Take, cancellationToken);
//    }
//}