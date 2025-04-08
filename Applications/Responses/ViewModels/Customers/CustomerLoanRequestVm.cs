namespace Applications.Responses.ViewModels.Customers;

public sealed class CustomerLoanRequest
{
    public string? LoanRequestId { get; set; }
    public DateTime? RequestDate { get; set; }
    public decimal? RequestAmount { get; set; } 
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public int? Installments { get; set; }
    public int Circle { get; set; } = 0;
    public string? InstallmentType { get; set; }
    public string? RepaymentMode { get; set; }
    public string? CurrencyName { get; set; }
    public string? Status { get; set; }
    public string? BranchName { get; set; }
    public string? InstallmentName { get; set; }
    public string? LoanCategoryName { get; set; } = string.Empty;
    public string? CollectionFeeModeName { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public string? Memo { get; set; }
}