using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Notifications.Dto;

public sealed class NotificationDto
{
    public Guid Id { get; set; }
    public string? UserId { get; set; } = default!;
    public int? BranchId { get; set; } = 0;
    public string? CustomerId { get; set; } = string.Empty;
    public string Type { get; set; } = default!;   // LoanRequest, LoanPayment
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string? ReferenceId { get; set; }        // LoanId / PaymentId
    public string? Url { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; }
    public string Icon => Type switch
    {
        "LoanRequest" => "account_balance",
        "LoanDisbursement" => "payments",
        "PaymentSchedule" => "info",
        _ => "notifications"
    };

    [NotMapped] public int RowIndex { get; set; } = 0;
}
