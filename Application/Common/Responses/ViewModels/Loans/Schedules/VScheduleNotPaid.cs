namespace Application.Responses.ViewModels.Loans.Schedules;

public sealed class VScheduleNotPaid
{
    public long CustomerId { get; init; }
    public Guid CustomerCode { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Phone { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public long? LoanId { get; init; }
    public long? CreditAgentId { get; set; } 
    public  int CurrencyId { get; set; }
    public long? LoanScheduleId { get; set; }
    public DateTime? StartDate { get; init; }
    public int? InstallmentNo { get; init; }
    public DateTime? PaymentDate { get; init; }
    public int? TotalDay { get; init; }
    public decimal? Principal { get; init; }
    public decimal? Interest { get; init; }
    public decimal? CollectionFee { get; init; }
    public decimal? PenaltyAmount { get; init; } 
    public decimal? TotalPay { get; init; }
    public decimal? PaidPrinciple { get; init; }
    public decimal? PaidInterest { get; init; }
    public decimal? TotalPaid { get; init; }
    public decimal? LoanOutstanding { get; init; }
    public string? ScheduleStatus { get; set; } = "Not Paid";
}
