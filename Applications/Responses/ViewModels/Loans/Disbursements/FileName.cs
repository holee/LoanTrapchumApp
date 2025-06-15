namespace Applications.Responses.ViewModels.Loans.Disbursements;

public sealed class VGuarantorLoan
{
    public string? EntityNameRefId { get; set; }
    public string? LoanId { get; set; }
    public string? LoanRequestId { get; set; }
    public string? CustomerName { get; set; } 
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime? StartDate { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? Amount { get; set; }
    public int? Installments { get; set; }
    public string? InstallmentType { get; set; }
    public int? CurrencyRefId { get; set; }
    public string? CurrencyName { get; set; }
    public string? BranchName { get; set; }
    public string? PeriodName { get; set; }
}
