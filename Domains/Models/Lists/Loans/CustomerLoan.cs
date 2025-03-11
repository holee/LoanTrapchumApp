namespace Domains.Models.Lists.Loans;

public class CustomerLoan
{
    public long? Id { get; set; }
    public Guid? LoanId { get; set; }
    public string? BranchName { get; set; }
    public DateTime RegisterDate { get; set; }
    public string? AgentName { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public int? CurrencyId { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? AdminFee { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? OutStanding { get; set; }
    public string? LoanStatus { get; set; }
    public int? Period { get; set; }
}
