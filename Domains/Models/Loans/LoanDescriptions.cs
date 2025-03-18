namespace Domains.Models.Loans;

public sealed class LoanDescriptions
{
    public long Id { get; set; }
    public Guid? LoanRequestRefId { get; set; }
    public string? Memo { get; set; }=string.Empty;
    public DateTime? CreatedDt { get; set; }
}
