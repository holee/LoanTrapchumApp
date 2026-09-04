using Applications.Commons.Mappers.Notifications;
using Applications.Extensions;
using Applications.Features.Notifications.Dto;
using Applications.Interfaces;
using Applications.Responses.Paginations;
using Domains.Enumerations;
using Domains.Models;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
namespace Infrastructures.Repository
{
    public sealed partial class NotificationRepository : INotificationService
    {
        private readonly DataContext _factory;
        private readonly ICurrentUserServices _userServices;
        private readonly IDapperConnectionFactory _connectionFactory;
        public NotificationRepository(DataContext factory, ICurrentUserServices cUserServices, IDapperConnectionFactory connectionFactory)
        {
            _factory = factory;
            _userServices = cUserServices;
            _connectionFactory = connectionFactory;
        }


        public async Task<Guid> CreateNofication(Notification notification, CancellationToken cancellation = default)
        {
            var db = _factory;

            var exists = await db.Notifications
                                .AnyAsync(x => x.UserId == notification.UserId &&
                                                x.BranchId == notification.BranchId &&
                                                x.ReferenceId == notification.ReferenceId &&
                                                x.IsRead == false, cancellation);
            if (exists)
            {
                return notification.Id;
            }

            db.Notifications.Add(notification);

            await db.SaveChangesAsync(cancellation);

            return notification.Id;

        }


        public async Task<int> GetUnreadCountAsync(string? userId, CancellationToken cancellation = default)
        {
            var db = _factory;
            return await db.Notifications
                .CountAsync(x => x.UserId == userId && !x.IsRead, cancellation);
        }


        public async Task<Pager<NotificationDto>> GetLatestAsync(string? userId, int? branchId, QueryParam Param, CancellationToken cancellation = default)
        {
            var db = _factory;

            await AddNotificationsByRoleAsync(userId, branchId, db, cancellation);

            var queries = db.Notifications
                                .Where(x => x.BranchId == branchId && x.IsRead == false)
                                .OrderByDescending(x => x.CreatedAt)
                                .AsNoTracking();

            queries = queries.WhereIf(!string.IsNullOrWhiteSpace(userId), x => x.UserId == userId);

            var total = await queries.CountAsync(cancellation);

            if (Param.Page > 0)
            {
                queries = queries
                             .Skip(Param.Page)
                             .Take(Param.Take);
            }

            var items = await queries
                                .Select(x => x.ToDto())
                                .ToListAsync(cancellation);
            return new Pager<NotificationDto>(items, total);
        }


        public async Task<Pager<NotificationDto>> GetLatestAsync(string? userId, int? branchId, NotificationStatus state, QueryParam param, CancellationToken cancellation = default)
        {
            var db = _factory;

            var query = BuildBaseQuery(db, branchId, userId, state, param);

            var total = await query.CountAsync(cancellation);

            var items = await query
                                .ApplyPaging(param)
                                .Select(x => x.ToDto())
                                .ToListAsync(cancellation);
            return new Pager<NotificationDto>(items, total);
        }


        public async Task<NotificationSummaryDto> GetNotificationSummaryAsync(string? userId, int? branchId, int take = 10, CancellationToken cancellation = default)
        {
            var db = _factory;

            var latest = await db.Notifications
                .Where(x => x.UserId == userId && !x.IsRead && x.BranchId == branchId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .Select(x => x.ToDto())
                .ToListAsync(cancellation);

            var counts = await db.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .GroupBy(x => x.Type)
                .Select(g => new NotificationTypeCountDto(g.Key, g.Count()))
                .ToListAsync(cancellation);

            return new NotificationSummaryDto(latest, counts);
        }

        public async Task<NotificationDto?> MarkAsReadAsync(Guid id, int? branchId, CancellationToken cancellation = default)
        {
            var db = _factory;

            var notification = await db.Notifications
                                        .FirstOrDefaultAsync(x => x.Id == id && x.BranchId == branchId, cancellation);

            if (notification is not null)
            {
                notification.IsRead = true;

                await db.SaveChangesAsync(cancellation);
            }

            return notification?.ToDto();
        }


        private async Task AddNotificationsByRoleAsync(string? userId, int? branchId, DataContext db, CancellationToken cancellation)
        {
            var role = _userServices.GetRole();

            if (IsAccountUser(role))
                await AddNotificationToAccountUser(userId, branchId, db, cancellation);

            if (IsCreditAgentUser(role))
                await AddNoficaitonToCreditAgentUser(userId, branchId, db, cancellation);

            if (IsManagerUser(role))
                await AddNotificationToManagerUser(userId, branchId, db, cancellation);
        }

        private IQueryable<Notification> BuildBaseQuery(DataContext db, int? branchId, string? userId, NotificationStatus status, QueryParam param)
        {
            var query = db.Notifications
                          .AsNoTracking()
                          .Where(x => x.BranchId == branchId)
                          .OrderByDescending(x => x.CreatedAt)
                          .AsQueryable();

            query = status switch
            {
                NotificationStatus.Read => query.Where(x => x.IsRead == true),

                NotificationStatus.Unread => query.Where(x => x.IsRead == false),

                _ => query
            };

            if (!string.IsNullOrWhiteSpace(userId))
                query = query.Where(x => x.UserId == userId);

            if (!string.IsNullOrWhiteSpace(param.Filter))
            {
                var filter = param.Filter.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Title, $"{filter}%") ||
                    EF.Functions.Like(x.Message, $"{filter}%") ||
                    EF.Functions.Like(x.Type.ToString(), $"{filter}%"));
            }

            return query;
        }



    }
}



//private async Task AddNotificationToManagerUser(string? userId, int? branchId, DataContext db, CancellationToken cancellation)
//{

//    var loanRequestToApprove = await db.VLoanRequests
//                                   .Where(x => x.BranchId == branchId &&
//                                               x.AprovedUserId == userId &&
//                                               (x.StatusId == (int)RequestStatus.Requested ||
//                                               x.StatusId == (int)RequestStatus.Checking ||
//                                                x.StatusId == (int)RequestStatus.ReApproved))
//                                   .AsNoTracking()
//                                   .ToListAsync(cancellation);

//    var requestIds = loanRequestToApprove.Select(x => x.LoanRequestId).ToList();

//    // 🔹 Load existing unread payment notifications
//    var existingKeys = await db.Notifications
//                            .Where(n =>
//                                n.Type == NotificationType.Request &&
//                                !n.IsRead &&
//                                requestIds.Contains(n.ReferenceId!) &&
//                                n.BranchId == branchId)
//                            .Select(n => new { n.ReferenceId, n.BranchId })
//                            .ToListAsync(cancellation);


//    var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
//                               .ToHashSet();

//    var notifications = loanRequestToApprove
//                   .Where(x => !existingSet.Contains((x.LoanRequestId, x.BranchId)))
//                   .Select(x => new Notification
//                   {
//                       UserId = userId,
//                       BranchId = x.BranchId,
//                       CustomerId = x.CustomerId,
//                       Type = NotificationType.Payment,
//                       Title = "Loan Payment Received",
//                       Message = $"A payment has been received for your loan '{x.LoanRequestId}'.",
//                       ReferenceId = x.LoanRequestId,
//                       Url = $"/App/Loans/Details/{x.LoanRequestId}/Customers/{x.CustomerId}",
//                       IsRead = false,
//                       CreatedAt = DateTime.Now
//                   })
//                   .ToList();



//    if (notifications.Count > 0)
//    {
//        await AddNotifications(db, notifications, cancellation);
//    }

//}

//private async Task AddNoficaitonToCreditAgentUser(string? userId, int? branchId, DataContext db, CancellationToken cancellation = default)
//{

//    var loanRequests = await db.VLoanRequests
//           .Where(x => x.BranchId == branchId &&
//                        x.UserRequestId == userId &&
//                        (x.StatusId == (int)RequestStatus.New ||
//                         x.StatusId == (int)RequestStatus.Rechecked))
//           .AsNoTracking()
//           .ToListAsync(cancellation);

//    var requestIdIds = loanRequests.Select(x => x.LoanRequestId).ToList();

//    // 🔹 Load existing unread payment notifications
//    var existingKeys = await db.Notifications
//                            .Where(n =>
//                                n.Type == NotificationType.Request &&
//                                !n.IsRead &&
//                                requestIdIds.Contains(n.ReferenceId!) &&
//                                n.BranchId == branchId)
//                            .Select(n => new { n.ReferenceId, n.BranchId })
//                            .ToListAsync(cancellation);

//    var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
//                                .ToHashSet();
//    var notifications = loanRequests
//                   .Where(x => !existingSet.Contains((x.LoanRequestId, x.BranchId)))
//                   .Select(x => new Notification
//                   {
//                       Title = "Receive Payment",
//                       Message = "Receive Payment",
//                       Type = NotificationType.Payment,
//                       ReferenceId = x.LoanRequestId,
//                       BranchId = x.BranchId,
//                       UserId = _userServices.GetUserId(),          // ✅ MUST be valid
//                       IsRead = false,
//                       Url = "",
//                       CreatedAt = DateTime.UtcNow
//                   })
//                   .ToList();
//    if (notifications.Count > 0)
//    {
//        await AddNotifications(db, notifications, cancellation);
//    }

//}

//private async Task AddNotificationToAccountUser(string? userId, int? branchId, DataContext db, CancellationToken cancellation)
//{
//    var approvedRequest = await GetApprovedLoanRequestAsNotifications(userId, branchId, db, cancellation);

//    var paymentSchedules = await GetSchedulesPaymentAsNotifications(db, branchId, cancellation);

//    var newNotifications = approvedRequest.Union(paymentSchedules);

//    if (newNotifications.Any())
//    {
//        await AddNotifications(db, newNotifications, cancellation);
//    }
//}

//private async Task AddNotifications(DataContext db, IEnumerable<Notification> notifications, CancellationToken cancellationToken = default)
//{
//    db.Notifications.AddRange(notifications);

//    await db.SaveChangesAsync(cancellationToken);

//}

//private static async Task<IEnumerable<Notification>> GetApprovedLoanRequestAsNotifications(string? userId, int? branchId, DataContext db, CancellationToken cancellation = default)
//{
//    var approvedLoanRequests = await db.VLoanRequests
//               .Where(x => x.BranchId == branchId && x.StatusId == (int)RequestStatus.Approved)
//               .AsNoTracking()
//               .ToListAsync(cancellation);

//    var approvedIds = approvedLoanRequests.Select(x => x.LoanRequestId).ToList();

//    var branchIds = approvedLoanRequests.Select(x => x.BranchId).ToList();

//    var existingKeys = await db.Notifications
//                          .Where(n =>
//                              n.Type == NotificationType.Payment &&
//                              !n.IsRead &&
//                              approvedIds.Contains(n.ReferenceId!) &&
//                              branchIds.Contains(n.BranchId))
//                          .Select(n => new { n.ReferenceId, n.BranchId })
//                          .ToListAsync(cancellation);



//    var existingKeySet = existingKeys.Select(x => (x.ReferenceId, x.BranchId)).ToHashSet();
//    var newNotifications = approvedLoanRequests
//        .Where(x => !existingKeySet.Contains((x.LoanRequestId, x.BranchId)))
//        .Select(x => new Notification
//        {
//            UserId = userId,
//            BranchId = x.BranchId,
//            CustomerId = x.CustomerId,
//            Type = NotificationType.Approved,
//            Title = "Loan Disbursement",
//            Message = $"Your loan request '{x.LoanRequestId}' has been approved and is ready for disbursement.",
//            ReferenceId = x.LoanRequestId,
//            Url = $"/App/Loans/Disbursements/{x.LoanRequestId}/Customers/{x.CustomerId}",
//            IsRead = false,
//            CreatedAt = DateTime.Now
//        })
//        .ToList();
//    return newNotifications ?? [];

//}

//private async Task<IEnumerable<Notification>> GetSchedulesPaymentAsNotifications(DataContext db, int? branchId, CancellationToken cancellationToken = default)
//{

//    var context = _factory;
//    var schedules = await GetLoanSchedulesByPaymentDateAsync(DateTimeOffset.Now, branchId, null, cancellationToken);
//    var loanIds = schedules.Select(x => x.LoanId).Distinct().ToArray();
//    // 🔹 Load existing unread payment notifications
//    var existingKeys = await context.Notifications
//                            .Where(n =>
//                                n.Type == NotificationType.Payment &&
//                                !n.IsRead &&
//                                loanIds.Contains(n.ReferenceId) &&
//                                 n.BranchId == branchId)
//                            .Select(n => new { n.ReferenceId, n.BranchId })
//                            .ToListAsync(cancellationToken);


//    var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
//                                .ToHashSet();

//    /// 🔹 Create new notifications for schedules without existing notifications
//    var notifications = schedules
//                   .Where(x => !existingSet.Contains((x.LoanId, x.BranchId)))
//                   .Select(x => new Notification
//                   {
//                       Title = "Receive Payment",
//                       Message = "Receive Payment",
//                       Type = NotificationType.Payment,
//                       ReferenceId = x.LoanId,
//                       BranchId = x.BranchId,
//                       UserId = _userServices.GetUserId(),          // ✅ MUST be valid
//                       IsRead = false,
//                       Url = "",
//                       CreatedAt = DateTime.UtcNow
//                   })
//                   .ToList();


//    return notifications ?? [];

//}

//private async Task<IEnumerable<VSchedule>> GetLoanSchedulesByPaymentDateAsync(DateTimeOffset? paymentDate, int? branchId, string? filter = null, CancellationToken cancellation = default)
//{

//    using var db = await _connectionFactory.CreateConnectionAsync(cancellation);

//    var parameters = new DynamicParameters();

//    parameters.Add("@BranchId", branchId ?? 0, DbType.Int32);

//    parameters.Add("@PaymentDate", paymentDate?.DateTime ?? DateTime.Now, DbType.DateTime);

//    parameters.Add("@Filter", filter ?? string.Empty, DbType.String);

//    var result = await db.QueryAsync<VSchedule>("spGetVSchedulesForNotifications",
//                            parameters,
//                            commandType: CommandType.StoredProcedure);

//    return result.ToList();

//}

//private static bool IsManagerUser(string? role)
//{
//    return default(AppRole).Only(AppRole.Manager, AppRole.ChiefBm, AppRole.Rbm, AppRole.Bm)
//                           .Contains(role);
//}

//private static bool IsCreditAgentUser(string? role)
//{
//    return default(AppRole).Only(AppRole.Co).Contains(role);
//}

//private static bool IsAccountUser(string? role)
//{
//    var accounts = default(AppRole).Only(AppRole.Account, AppRole.ChiefAccount, AppRole.SeniorAccount, AppRole.Teller);

//    return accounts.Contains(role);

//}


//private async Task AddNotifications(IEnumerable<VSchedule> schedules, CancellationToken cancellationToken = default)
//{

//    if (!schedules.Any())
//        return;

//    var userId = _userServices.UserId;

//    var context = _factory;

//    var loanIds = schedules.Select(x => x.LoanId).Distinct().ToArray();

//    var branchIds = schedules.Select(x => x.BranchId).Distinct().ToArray();

//    // 🔹 Load existing unread payment notifications
//    var existingKeys = await context.Notifications
//                            .Where(n =>
//                                n.Type == NotificationType.Payment &&
//                                !n.IsRead &&
//                                loanIds.Contains(n.ReferenceId) &&
//                                branchIds.Contains(n.BranchId))
//                            .Select(n => new { n.ReferenceId, n.BranchId })
//                            .ToListAsync(cancellationToken);


//    var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
//                                .ToHashSet();


//    var notifications = schedules
//                   .Where(x => !existingSet.Contains((x.LoanId, x.BranchId)))
//                   .Select(x => new Notification
//                   {
//                       Title = "Receive Payment",
//                       Message = "Receive Payment",
//                       Type = NotificationType.Payment,
//                       ReferenceId = x.LoanId,
//                       BranchId = x.BranchId,
//                       UserId = userId,          // ✅ MUST be valid
//                       IsRead = false,
//                       CreatedAt = DateTime.UtcNow
//                   })
//                   .ToList();


//    if (notifications.Count == 0)
//        return;

//    context.Notifications.AddRange(notifications);

//    await context.SaveChangesAsync(cancellationToken);

//}
