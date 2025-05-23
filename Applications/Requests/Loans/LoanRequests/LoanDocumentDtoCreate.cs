namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanDocumentDtoCreate
{
    public long Id { get; set; }
    public int? BranchId { get; set; }
    public string? LoanRequestId { get; set; }
    public string? OriginalName { get; set; }
    public string? NewFileName { get; set; }
    public string? AltText { get; set; }
    public long? FileSize { get; set; }=0;
    public string? FileUrl { get; set; }
    public string? Description { get; set; }
}

public sealed class LoanDocumentDtoUpdate 
{
    public long Id { get; set; }
    public int? BranchId { get; set; }
    public string? LoanRequestId { get; set; }
    public string? OriginalName { get; set; }
    public string? NewFileName { get; set; }
    public string? AltText { get; set; }
    public string? FileUrl { get; set; }
    public long? FileSize { get; set; } = 0;
    public string? Description { get; set; }
}