namespace Domains.Models.Loans;

public sealed class LoanDescriptions
{
    public long Id { get; set; }
    public string? LoanRequestRefId { get; set; }
    public string? UserRefId { get; set; } 
    public string? Memo { get; set; }=string.Empty;
    public DateTime? CreatedDt { get; set; }
}
