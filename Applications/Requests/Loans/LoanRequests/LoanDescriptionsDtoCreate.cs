namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanDescriptionsDtoCreate
{
    public long Id { get; set; }
    public string? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}