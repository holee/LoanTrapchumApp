using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Dto
{
    public sealed class NotificationDtoUpdate
    { 
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public int BranchId { get; set; }
        public string Type { get; set; } = default!;   // LoanRequest, LoanPayment
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? ReferenceId { get; set; }        // LoanId / PaymentId
        public string? Url { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
