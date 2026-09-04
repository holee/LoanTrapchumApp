namespace Application.Responses.ViewModels.Customers;

public sealed class VCustomerLoans 
{
    public string? CustomerId { get; set; }
    public string? LoanId { get; set; }
    public string? BranchName { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public string? InstallmentType { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? AdminFee { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? OutStanding { get; set; }
    public decimal? TotalInterest { get; set; }
    public decimal? TotalCollectionFee { get; set; }
    public decimal? TotalOtherCharge { get; set; }
    public DateTime? RegisterDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? LoanStatus { get; set; }
}
