using Applications.Features.Notifications.Dto;
using Domains.Enumerations;
using Domains.Models;

namespace Applications.Interfaces
{
    public interface INotificationService
    {
        Task<Guid> CreateNofication(Notification notification, CancellationToken cancellation = default);
        Task<Pager<NotificationDto>> GetLatestAsync(string? userId, int? branchId, QueryParam param, CancellationToken cancellation = default);
        Task<Pager<NotificationDto>> GetLatestAsync(string? userId, int? branchId, NotificationStatus state, QueryParam param, CancellationToken cancellation = default);
        Task<NotificationSummaryDto> GetNotificationSummaryAsync(string? userId, int? branchId, int take = 10, CancellationToken cancellation = default);
        Task<int> GetUnreadCountAsync(string? userId, CancellationToken cancellation = default);
        Task<NotificationDto?> MarkAsReadAsync(Guid id, int? branchId, CancellationToken cancellation = default);
    }
}
