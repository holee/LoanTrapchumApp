namespace Applications.Responses.ViewModels.Loans.Disbursements;

public sealed class VGuarantorLoan
{
    public long Id { get; set; }
    public string? GuarantorId { get; set; }
    public string? LoanId { get; set; }
    public string? LoanRequestId { get; set; }
    public string? CustomerName { get; set; } 
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    //public DateTime? StartDate { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal PaidPrinciple { get; set; }   
    public decimal PaidInterest { get; set; }   
    public decimal OutStanding { get; set; }     
    public string? CreditAgentKhmer { get; set; } 
    public string? AgentName { get; set; } 
    public string? InstallmentType { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public string? BranchName { get; set; }
    public string? PeriodName { get; set; }
    public string? Remark { get; set; }
}
