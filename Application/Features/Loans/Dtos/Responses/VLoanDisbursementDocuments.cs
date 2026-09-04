using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Loans.Dtos.Responses;

public sealed class VLoanDisbursementDocuments 
{
    public long Id { get; set; }
    public string? LoanRequestId { get; set; }
    public string? LoanId { get; set; }
    public int? BranchRefId { get; set; }
    public string? OriginalName { get; set; }
    public string? NewFileName { get; set; }
    public string? FileUrl { get; set; }
    public string? AltText { get; set; }
    public string? Description { get; set; }
    public long? FileSize { get; set; }
    public DateTime CreatedUtc { get; set; }
    [NotMapped] public int RowIndex { get; set; } = 1; 
}