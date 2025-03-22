namespace Applications.Requests.Loans.Disbursements;

public sealed class LoanDecriptionDtoCreate
{
    public long? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
