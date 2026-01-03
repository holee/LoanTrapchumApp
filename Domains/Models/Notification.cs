using Domains.Enumerations;

namespace Domains.Models
{
    public sealed class Notification
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; } = default!;
        public int? BranchId { get; set; }
        public string? ReferenceUserId { get; set; }
        public string? CustomerId { get; set; } = string.Empty;
        public NotificationType Type { get; set; } = NotificationType.All;   // LoanRequest, LoanPayment
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? ReferenceId { get; set; }        // LoanId / PaymentId
        public string? Url { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
