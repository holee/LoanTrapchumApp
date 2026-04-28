using Domains.Models.Bases;
using Domains.Models.Lists;

namespace Domains.Models.Loans;

public sealed class CoLoanRequest
{
    public Guid Id { get; set; }
    public string? RequestId { get; set; }
    public string? CuctomerId { get; set; }
    public string? LoanId { get; set; }
    public bool? Status { get; set; }
    public DateTimeOffset? CreatedDT { get; set; }
    public DateTimeOffset? ModifiedDT { get; set; }
    public string? Description { get; set; }
    public LoanRequest? LoanRequest { get; set; }
    public EntityNames? Customer { get; set; }
}
