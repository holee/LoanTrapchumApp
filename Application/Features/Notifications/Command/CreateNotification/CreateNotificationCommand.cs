using Application.Features.Notifications.Dto;
using ErrorOr;
namespace Application.Features.Notifications.Command.CreateNotification;

public record CreateNotificationCommand(NotificationDtoCreate Request) : IRequest<ErrorOr<Guid>>;
