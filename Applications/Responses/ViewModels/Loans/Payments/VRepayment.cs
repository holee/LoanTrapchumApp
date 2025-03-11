namespace Applications.Responses.ViewModels.Loans.Payments;

public sealed class VSchedulePaid
{
    public long? LoanId { get; set; }
    public int BranchId { get; set; }
    public long? LoanScheduleId { get; set; }
    public int? InstallmentNo { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principal { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? OtherCharge { get; set; }
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
    public int? StatusId { get; set; }
    public string? Status { get; set; }
}
