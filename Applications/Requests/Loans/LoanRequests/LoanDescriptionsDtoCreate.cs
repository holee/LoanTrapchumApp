namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanDescriptionsDtoCreate
{
    public string? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}

public sealed class LoanDescriptionsDtoUpdate 
{
    public long Id { get; set; }
    public string? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}