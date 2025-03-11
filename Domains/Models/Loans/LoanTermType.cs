namespace Domains.Models.Loans;

public class LoanTermType
{
    public int LoanTermId { get; set; }
    public string? LoanTermCode { get; set; } = string.Empty;
    public string? Description { get; set; }=string.Empty;
}
