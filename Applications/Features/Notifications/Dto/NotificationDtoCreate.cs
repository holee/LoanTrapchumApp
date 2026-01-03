namespace Applications.Features.Notifications.Dto;

public sealed class NotificationDtoCreate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string? UserId { get; set; }
    public required int? BranchId { get; set; }
    public required string? ReferenceId { get; set; }        // LoanId / PaymentId
    public string? CustomerId { get; set; } = string.Empty;
    public string Type { get; set; } = default!;   // LoanRequest, LoanPayment
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string? Url { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }
}
