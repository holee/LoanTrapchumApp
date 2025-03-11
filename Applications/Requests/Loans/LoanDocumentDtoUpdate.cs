namespace Applications.Requests.Loans;

public sealed class LoanDocumentDtoUpdate 
{
    public long Id { get; set; }
    public long LoanRequestId { get; set; }
    public string? DocumentName { get; set; }
    public string? AltText { get; set; }
    public string? Description { get; set; }
}