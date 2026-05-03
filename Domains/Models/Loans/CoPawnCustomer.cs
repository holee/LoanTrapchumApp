namespace Domains.Models.Loans;

public sealed class CoPawnCustomer
{
    public Guid Id { get; set; }
    public int? BranchId { get; set; } = 0;
    public string? RequestId { get; set; }
    public string? CustomerId { get; set; }
    public string? LoanId { get; set; }
    public bool? Status { get; set; } = true;
    public string? Description { get; set; }
    public DateTimeOffset? CreatedDT { get; set; }
    public DateTimeOffset? ModifiedDT { get; set; }
    public LoanRequest? LoanRequest { get; set; }
}
