namespace Domains.Models.Loans;

public class LoanSubTypeStatus
{
    public int LoanSubTypeId { get; set; }
    public string? Description { get; set; }
    public int? LoanTypeStatusRefId { get; set; }
}
