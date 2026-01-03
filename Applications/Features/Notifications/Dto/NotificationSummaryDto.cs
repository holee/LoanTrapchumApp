namespace Applications.Features.Notifications.Dto;

public sealed record NotificationSummaryDto(List<NotificationDto> Latest, List<NotificationTypeCountDto> Counts);
