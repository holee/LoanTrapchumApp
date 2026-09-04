using Domains.Enumerations;

namespace Application.Features.Notifications.Dto;

public sealed record NotificationTypeCountDto(NotificationType Type, int Count);
