namespace Applications.Responses.ViewModels.Loans.Schedules;

public class VScheduleVm
{
    public long ScheduleId { get; set; }
    public int? No { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? TotalDays { get; set; }
    public decimal? PaidPrincipal { get; set; }
    public decimal? PaidInterest { get; set; }
    public decimal? PaidAdminFee { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? OutStanding { get; set; }
    public bool IsPaid { get; set; } = false;
    public string? ScheduleStatus { get; set; }
}