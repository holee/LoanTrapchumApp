namespace Domains.Models.Loans;

public sealed class LoanDocuments
{
    public long Id { get; set; }
    public int? BranchId { get; set; }
    public string? LoanRequestId { get; set; }
    public string? OriginalName { get; set; }
    public string? NewFileName { get; set; }
    public string? FileUrl { get; set; }
    public string? AltText { get; set; }
    public long? FileSize { get; set; }
    public string? Description { get; set; }
    public DateTime? Created { get; set; } = DateTime.Now; 
    public LoanRequest? LoanRequest { get; set; }
}
