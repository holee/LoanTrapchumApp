namespace Domains.Models.Loans;

public sealed class LoanRequestSchedule
{
    public long LoanRequestScheduleId { get; set; }
    public string? LoanRequestRefId { get; set; }
    public int? InstallmentNo { get; set; }
    public decimal? Principal { get; set; }
    public decimal? InterestAmount { get; set; }
    public decimal? CollectionFeeAmount { get; set; }
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
    public LoanRequest? LoanRequest { get; set; }
}
