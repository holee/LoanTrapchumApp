namespace Application.Responses.ViewModels.Loans.Schedules;

public class VScheduleBase
{
    public long? LoanScheduleId { get; set; }
    public long? CreditAgentId { get; set; }
    public string? CreditAgentNameKh { get; set; }
    public string? CreditAgentNameEn { get; set; }
    public long? CustomerId { get; set; }
    public Guid? CustomerCode { get; set; }
    public string? FullName { get; set; }
    public string? AltName { get; set; }
    public long? LoanId { get; set; }
    public int BranchId { get; set; }
    public int? CurrencyId { get; set; }
    public int? InstallmentNo { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principal { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? PenaltyRate { get; set; }
    public int? DayArrea { get; set; } = 0;
    public decimal? PenaltyAmount { get; set; } = 0;
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
}
