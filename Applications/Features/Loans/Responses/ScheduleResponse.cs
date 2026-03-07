namespace Applications.Features.Loans.Responses;

public sealed class ScheduleResponse
{
    public int RowNum { get; set; } = 1;
    public long? LoanScheduleId { get; set; }
    public int? CurrencyId { get; set; }
    public int? InstallmentNo { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? TotalFee { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
    public int? ScheduleNum { get; set; } = 1;
    public int RowIndex { get; set; } = 1;
}
