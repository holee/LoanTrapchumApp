namespace Application.Responses.ViewModels.Loans.Requests;

public sealed class VCustomerLoanRequests 
{
    public string? LoanRequestId { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? CreditAgentId { get; set; }
    public decimal? RequestAmount { get; set; }
    public decimal? AprovedAmount { get; set; }
    public string? AprovedBy { get; set; }
    public DateTime? AprovedDate { get; set; }
    public int? RequestCurrencyId { get; set; }
    public string? RequestCurrencyName { get; set; }
    public string? AltName { get; set; }
    public int? RequestBranchId { get; set; }
    public string? RequestBranchName { get; set; }
    public string? Status { get; set; }
}
