namespace Applications.Responses.ViewModels.Loans.Requests;

public sealed class ApprovedRequest
{
    public string? LoanRequestId { get; set; }
    public string? CustomerId { get; set; }
    public string? AltName { get; set; }
    public int BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? RequestUserId { get; set; }
    public string? Summiter { get; set; }
    public string? AprovedUserId { get; set; }
    public string? AprovedBy { get; set; }
    public decimal? RequestAmount { get; set; } = 0;
    public decimal? AprovedAmount { get; set; } = 0;
    public decimal? AdminFeeAmount { get; set; } = 0;

}