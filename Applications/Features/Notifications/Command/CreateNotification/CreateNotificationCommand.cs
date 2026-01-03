using Applications.Features.Notifications.Dto;
using ErrorOr;
namespace Applications.Features.Notifications.Command.CreateNotification;

public record CreateNotificationCommand(NotificationDtoCreate Request) : IRequest<ErrorOr<Guid>>;
