namespace Applications.Requests.Loans.Disbursements;

public sealed class LoanDecriptionDtoCreate
{
    public Guid? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
