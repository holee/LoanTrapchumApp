namespace Applications.Responses.ViewModels.Employees;

public sealed class VEmployeeLoans 
{
    public string? LoanId { get; set; }
    public string? EmployeeId { get; set; }
    public DateTime? RegisterDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal? Principle { get; set; }
    public float? InterestRate { get; set; }
    public float? AdminFeeRate { get; set; }
    public float? CollectionFeeRate { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? PaidInterest { get; set; }
    public decimal? PaidCollectionFee { get; set; }
    public decimal? OutStanding { get; set; }
    public string? PeriodName { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public string? BranchName { get; set; }
    public string? LoanStatus { get; set; }
}
