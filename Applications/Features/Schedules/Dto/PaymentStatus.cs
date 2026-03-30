namespace Applications.Features.Schedules.Dto;

public sealed class PaymentStatus
{
    public long? LoanScheduleId { get; set; }
    public decimal? PaidPrinciple { get; set; } = 0;
    public decimal? PaidInterest { get; set; }= 0;
    public decimal? PaidCollectionFee { get; set; } = 0;
    public decimal? PaidAmount { get; set; } = 0;
    public decimal? LeftInterestAmount { get; set; } = 0;
    public decimal? LeftCollectionFeeAmount { get; set; } = 0;
    public decimal? LeftPrinciple { get; set; } = 0;
    public decimal? LeftAmount { get; set; } = 0;
}
