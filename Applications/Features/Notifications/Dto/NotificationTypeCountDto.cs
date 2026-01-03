using Domains.Enumerations;

namespace Applications.Features.Notifications.Dto;

public sealed record NotificationTypeCountDto(NotificationType Type, int Count);
