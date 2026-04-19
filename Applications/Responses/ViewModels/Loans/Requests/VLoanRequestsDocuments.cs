using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Loans.Requests;

public sealed class VLoanRequestsDocuments
{
    public long Id { get; set; }
    public string? LoanRequestId { get; set; }
    public int? BranchRefId { get; set; }
    public Guid? DocumentId { get; set; } = Guid.Empty;
    public string? OriginalName { get; set; }
    public string? NewFileName { get; set; }
    public string? FileUrl { get; set; }
    public string? AltText { get; set; }
    public string? Description { get; set; }
    public long? FileSize { get; set; }
    public DateTime CreatedUtc { get; set; }
    [NotMapped] public int RowIndex { get; set; } = 1;
}
