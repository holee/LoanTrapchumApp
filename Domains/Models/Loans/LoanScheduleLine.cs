namespace Domains.Models.Loans;

public sealed class ScheduleLine
{
    public int InstallmentNo { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public int TotalDay { get; set; } = 0;
    public decimal? Principal { get; set; } = 0;
    public decimal? Interest { get; set; } = 0;
    public decimal? CollectionFeeAmount { get; set; } = 0;
    public decimal? TotalPay { get; set; } = 0;
    public decimal? OutStanding { get; set; } = 0;
    public int? RepaymentModeRefId { get; set; } = 0;
    public int? ScheduleStatusRefId { get; set; } = 0;
    public int? BranchId { get; set; }
    public int? ScheduleNum { get; set; } = 1;
    public string? CreditAgentRefId { get; set; }=string.Empty;
}
