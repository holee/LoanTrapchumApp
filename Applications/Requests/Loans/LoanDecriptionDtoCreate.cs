namespace Applications.Requests.Loans;

public sealed class LoanDecriptionDtoCreate
{
    public long? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
