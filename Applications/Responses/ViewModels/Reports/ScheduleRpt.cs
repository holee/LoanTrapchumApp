using MiniExcelLibs.Attributes;

namespace Applications.Responses.ViewModels.Reports;

public sealed class ScheduleRpt
{
    public int? InstallmentNo { get; set; }

    [ExcelFormat("dd/MMM/yyyy")]
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? TotalInterest => Interest + CollectionFee;
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
}
