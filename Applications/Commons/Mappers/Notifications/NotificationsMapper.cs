using Applications.Features.Notifications.Dto;
using Domains.Enumerations;
using Domains.Models;

namespace Applications.Commons.Mappers.Notifications
{
    public static class NotificationsMapper
    {
        public static NotificationDto ToDto(this Notification source)
        {
            return new NotificationDto
            {
                Id = source.Id,
                Type = source.Type.ToString(),
                Message = source.Message,
                Url = source.Url,
                UserId = source.UserId,
                CreatedAt = source.CreatedAt,
                IsRead = source.IsRead,
                ReferenceId = source.ReferenceId,
                Title = source.Title,
                CustomerId = source.CustomerId,
                BranchId = source.BranchId,
            };
        }

        public static List<NotificationDto> ToListDto(this List<Notification> sources)
        {
            return sources.Select(x => x.ToDto()).ToList();
        }


        public static Notification MapToEntity(this NotificationDto source)
        {
            return new Notification
            {
                Id = source.Id,
                Type = Enum.Parse<NotificationType>(source.Type, ignoreCase: true),
                Message = source.Message,
                Url = source.Url,
                UserId = source.UserId,
                CreatedAt = source.CreatedAt,
                IsRead = source.IsRead,
                ReferenceId = source.ReferenceId,
                Title = source.Title,
                BranchId = source.BranchId,
                CustomerId = source.CustomerId,
            };
        }


        public static Notification ToDtoUpdate(this NotificationDto source)
        {
            return new Notification
            {
                Id = source.Id,
                Type = Enum.Parse<NotificationType>(source.Type, ignoreCase: true),
                Message = source.Message,
                Url = source.Url,
                UserId = source.UserId,
                CreatedAt = source.CreatedAt,
                IsRead = source.IsRead,
                ReferenceId = source.ReferenceId,
                Title = source.Title,
                CustomerId = source.CustomerId,
                BranchId = source.BranchId,
            };
        }

        public static Notification ToDtoCreate(this NotificationDtoCreate source)
        {
            return new Notification
            {
                Id = source.Id,
                Type = Enum.Parse<NotificationType>(source.Type, ignoreCase: true),
                Message = source.Message,
                Url = source.Url,
                UserId = source.UserId,
                CreatedAt = source.CreatedAt,
                IsRead = source.IsRead,
                ReferenceId = source.ReferenceId,
                Title = source.Title,
                BranchId = source.BranchId,
                CustomerId = source.CustomerId,
            };
        }

        public static NotificationDto ToDtoAddNotification(this NotificationDtoCreate source)
        {
            return new NotificationDto
            {
                Id = source.Id,
                Type = source.Type,
                Message = source.Message,
                Url = source.Url,
                UserId = source.UserId,
                CreatedAt = source.CreatedAt,
                IsRead = source.IsRead,
                ReferenceId = source.ReferenceId,
                Title = source.Title,
                BranchId = source.BranchId,
                CustomerId = source.CustomerId,
            };
        }

    }
}
