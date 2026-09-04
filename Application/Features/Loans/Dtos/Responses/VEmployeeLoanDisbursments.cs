namespace Application.Features.Loans.Dtos.Responses;

public class VEmployeeLoanDisbursments   
{
    public string? LoanId { get; set; }
    public string? CreditAgentRefId { get; set; }
    public DateTime RegisterDate { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public string? LoanBranchName { get; set; }
    public decimal? Amount { get; set; }
    public int? LoanCurrencyId { get; set; }
    public string? LoanCurrencyName { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public int? Circle { get; set; }
    public string? InstallmentType { get; set; }
    public string? CustomerName { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? PaidInterest { get; set; }
    public decimal? PaidCollectionFee { get; set; }
    public decimal? OutStanding { get; set; }
    public string? LoanStatus { get; set; }
}
