namespace Applications.Responses.ViewModels.Reports;

public sealed class LoanScheduleRpt 
{
    public string? ContractNumber { get; set; }
    public Guid? LoanId { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? AltName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public DateTime RegisterDate { get; set; }
    public string? AgentName { get; set; }
    public string? AgentPhone { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? AdminFee { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? OutStanding { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public string? Period { get; set; }
}
