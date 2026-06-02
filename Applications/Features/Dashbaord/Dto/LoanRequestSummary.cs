namespace Applications.Features.Dashbaord.Dto;

public sealed class LoanRequestSummary
{
    public int TotalRequests { get; set; } = 0;
    public int? CurrencyId { get; set; } = 0;
    public int? BranchId { get; set; } = 0;
    public string? Status { get; set; } = string.Empty;
}
