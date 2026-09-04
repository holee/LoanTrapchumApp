namespace Application.Features.Reports.Dtos;

public sealed class RptScheduleDetailsLine
{
    public int RowNum { get; set; } = 1;
    public int? CurrencyId { get; set; }
    public int? InstallmentNo { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFee { get; set; }

    public decimal? TotalFee { get; set; } = 0;
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
}
