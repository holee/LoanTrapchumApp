using Application.Interfaces;
using Application.Commons.Mappers.Notifications;
using ErrorOr;
namespace Application.Features.Notifications.Command.CreateNotification;

public sealed class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, ErrorOr<Guid>>
{
    private readonly INotificationService _notificationService;
    public CreateNotificationCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public async Task<ErrorOr<Guid>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notificationEntity = request.Request.ToDtoCreate();

        return await _notificationService.CreateNofication(notificationEntity);
    }
}
