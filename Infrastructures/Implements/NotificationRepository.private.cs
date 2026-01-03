using Applications.Enumerations;
using Applications.Extensions.Identity.Enumerations;
using Applications.Interfaces;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Enumerations;
using Domains.Models;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Infrastructures.Implements
{
    public partial class NotificationRepository : INotificationService
    {
        private async Task AddNotificationToManagerUser(string? userId, int? branchId, DataContext db, CancellationToken cancellation)
        {

            var loanRequestToApprove = await db.VLoanRequests
                                           .Where(x => x.BranchId == branchId &&
                                                       x.AprovedUserId == userId &&
                                                       (x.StatusId == (int)RequestStatus.Requested ||
                                                       x.StatusId == (int)RequestStatus.Checking ||
                                                        x.StatusId == (int)RequestStatus.ReApproved))
                                           .AsNoTracking()
                                           .ToListAsync(cancellation);

            var requestIds = loanRequestToApprove.Select(x => x.LoanRequestId).ToList();

            // 🔹 Load existing unread payment notifications
            var existingKeys = await db.Notifications
                                    .Where(n =>
                                        n.UserId == userId &&
                                        n.Type == NotificationType.Request && !n.IsRead &&
                                        requestIds.Contains(n.ReferenceId!) &&
                                        n.BranchId == branchId)
                                    .Select(n => new { n.ReferenceId, n.BranchId })
                                    .ToListAsync(cancellation);


            var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
                                       .ToHashSet();

            var notifications = loanRequestToApprove
                           .Where(x => !existingSet.Contains((x.LoanRequestId, x.BranchId)))
                           .Select(x => new Notification
                           {
                               UserId = userId,
                               BranchId = x.BranchId,
                               CustomerId = x.CustomerId,
                               Type = NotificationType.Request,
                               Title = "Loan Request",
                               Message = $"A request has been received for your loan '{x.LoanRequestId}'.",
                               ReferenceId = x.LoanRequestId,
                               Url = $"/App/Loans/{x.LoanRequestId}/Requests/{x.BranchId}/Branches/{x.CustomerId}/Details",
                               IsRead = false,
                               CreatedAt = DateTime.Now
                           })
                           .ToList();



            if (notifications.Count > 0)
            {
                await AddNotifications(db, notifications, cancellation);
            }

        }

        private async Task AddNoficaitonToCreditAgentUser(string? userId, int? branchId, DataContext db, CancellationToken cancellation = default)
        {

            var loanRequests = await db.VLoanRequests
                   .Where(x => x.BranchId == branchId &&
                                x.UserRequestId == userId &&
                                (x.StatusId == (int)RequestStatus.New ||
                                 x.StatusId == (int)RequestStatus.Rechecked))
                   .AsNoTracking()
                   .ToListAsync(cancellation);

            var requestIdIds = loanRequests.Select(x => x.LoanRequestId).ToList();

            // 🔹 Load existing unread payment notifications
            var existingKeys = await db.Notifications
                                    .Where(n =>
                                        n.UserId == userId &&
                                        n.Type == NotificationType.Request && !n.IsRead &&
                                        requestIdIds.Contains(n.ReferenceId!) &&
                                        n.BranchId == branchId)
                                    .Select(n => new { n.ReferenceId, n.BranchId })
                                    .ToListAsync(cancellation);

            var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
                                        .ToHashSet();
            var notifications = loanRequests
                           .Where(x => !existingSet.Contains((x.LoanRequestId, x.BranchId)))
                           .Select(x => new Notification
                           {
                               Title = "Loan Request",
                               Message = "Loan Request",
                               Type = NotificationType.Request,
                               ReferenceId = x.LoanRequestId,
                               BranchId = x.BranchId,
                               UserId = _userServices.GetUserId(),          // ✅ MUST be valid
                               IsRead = false,
                               CustomerId = x.CustomerId,
                               Url = $"/App/Loans/{x.LoanRequestId}/Requests/{x.BranchId}/Branches/{x.CustomerId}/Details",
                               CreatedAt = DateTime.UtcNow
                           })
                           .ToList();
            if (notifications.Count > 0)
            {
                await AddNotifications(db, notifications, cancellation);
            }

        }

        private async Task AddNotificationToAccountUser(string? userId, int? branchId, DataContext db, CancellationToken cancellation)
        {
            var approvedRequest = await GetApprovedLoanRequestAsNotifications(userId, branchId, db, cancellation);

            var paymentSchedules = await GetSchedulesPaymentAsNotifications(db, branchId, cancellation);

            var newNotifications = approvedRequest.Union(paymentSchedules);

            if (newNotifications.Any())
            {
                await AddNotifications(db, newNotifications, cancellation);
            }
        }

        private async Task AddNotifications(DataContext db, IEnumerable<Notification> notifications, CancellationToken cancellationToken = default)
        {
            db.Notifications.AddRange(notifications);

            await db.SaveChangesAsync(cancellationToken);

        }

        private static async Task<IEnumerable<Notification>> GetApprovedLoanRequestAsNotifications(string? userId, int? branchId, DataContext db, CancellationToken cancellation = default)
        {
            var approvedLoanRequests = await db.VLoanRequests
                       .Where(x => x.BranchId == branchId && x.StatusId == (int)RequestStatus.Approved)
                       .AsNoTracking()
                       .ToListAsync(cancellation);

            var approvedIds = approvedLoanRequests.Select(x => x.LoanRequestId).ToList();

            var branchIds = approvedLoanRequests.Select(x => x.BranchId).ToList();

            var existingKeys = await db.Notifications
                                  .Where(n =>
                                      n.Type == NotificationType.Payment &&
                                      !n.IsRead &&
                                      approvedIds.Contains(n.ReferenceId!) &&
                                      branchIds.Contains(n.BranchId))
                                  .Select(n => new { n.ReferenceId, n.BranchId })
                                  .ToListAsync(cancellation);



            var existingKeySet = existingKeys.Select(x => (x.ReferenceId, x.BranchId)).ToHashSet();
            var newNotifications = approvedLoanRequests
                .Where(x => !existingKeySet.Contains((x.LoanRequestId, x.BranchId)))
                .Select(x => new Notification
                {
                    UserId = userId,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    Type = NotificationType.Approved,
                    Title = "Loan Disbursement",
                    Message = $"Your loan request '{x.LoanRequestId}' has been approved and is ready for disbursement.",
                    ReferenceId = x.LoanRequestId,
                    Url = $"App/Loans/Disbursements/{x.LoanRequestId}/Branches/{x.BranchId}?CustomerId={x.CustomerId}",
                    IsRead = false,
                    CreatedAt = DateTime.Now
                })
                .ToList();
            return newNotifications ?? [];
            //
        }

        private async Task<IEnumerable<Notification>> GetSchedulesPaymentAsNotifications(DataContext db, int? branchId, CancellationToken cancellationToken = default)
        {

            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            var schedules = await GetLoanSchedulesByPaymentDateAsync(DateTimeOffset.Now, branchId, null, cancellationToken);
            var loanIds = schedules.Select(x => x.LoanId).Distinct().ToArray();
            // 🔹 Load existing unread payment notifications
            var existingKeys = await context.Notifications
                                    .Where(n =>
                                        n.Type == NotificationType.Payment &&
                                        !n.IsRead &&
                                        loanIds.Contains(n.ReferenceId) &&
                                         n.BranchId == branchId)
                                    .Select(n => new { n.ReferenceId, n.BranchId })
                                    .ToListAsync(cancellationToken);


            var existingSet = existingKeys.Select(x => (x.ReferenceId, x.BranchId))
                                        .ToHashSet();

            /// 🔹 Create new notifications for schedules without existing notifications
            var notifications = schedules
                           .Where(x => !existingSet.Contains((x.LoanId, x.BranchId)))
                           .Select(x => new Notification
                           {
                               Title = "Receive Payment",
                               Message = "Receive Payment",
                               Type = NotificationType.Payment,
                               ReferenceId = x.LoanId,
                               BranchId = x.BranchId,
                               UserId = _userServices.GetUserId(),        
                               IsRead = false,
                               CustomerId = x.CustomerId,
                               Url = $"/App/Loans/{x.BranchId}/Branches/{x.LoanId}/Schedules",
                               CreatedAt = DateTime.UtcNow
                           })
                           .ToList();


            return notifications ?? [];

        }

        private async Task<IEnumerable<VSchedule>> GetLoanSchedulesByPaymentDateAsync(DateTimeOffset? paymentDate, int? branchId, string? filter = null, CancellationToken cancellation = default)
        {

            using var db = await _connectionFactory.CreateConnectionAsync(cancellation);

            var parameters = new DynamicParameters();

            parameters.Add("@BranchId", branchId ?? 0, DbType.Int32);

            parameters.Add("@PaymentDate", paymentDate?.DateTime ?? DateTime.Now, DbType.DateTime);

            parameters.Add("@Filter", filter ?? string.Empty, DbType.String);

            var result = await db.QueryAsync<VSchedule>("spGetVSchedulesForNotifications",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            return result.ToList();

        }

        private static bool IsManagerUser(string? role)
        {
            return default(AppRole).Only(AppRole.Manager, AppRole.ChiefBm, AppRole.Rbm, AppRole.Bm)
                                   .Contains(role);
        }

        private static bool IsCreditAgentUser(string? role)
        {
            return default(AppRole).Only(AppRole.Co).Contains(role);
        }

        private static bool IsAccountUser(string? role)
        {
            var accounts = default(AppRole).Only(AppRole.Account, AppRole.ChiefAccount, AppRole.SeniorAccount, AppRole.Teller);

            return accounts.Contains(role);

        }


    }
}


//private async Task AddNotifications(IEnumerable<VSchedule> schedules, CancellationToken cancellationToken = default)
//{

//    if (!schedules.Any())
//        return;

//    var userId = _userServices.UserId;

//    await using var context = await _factory.CreateDbContextAsync(cancellationToken);

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
