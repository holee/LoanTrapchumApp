using Domains.Models.Loans;
namespace Domains.Models.Collaterals;

public sealed class CollateralLink
{

    public long Id { get; set; }
    public int? BranchId { get; set; }
    public long? CollateralLineRefId { get; set; }
    public string? LoanRefId { get; set; }
    public string? LoanRequestRefId { get; set; }
    public string? CollateralRefId { get; set; }
    public bool? IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
    public DateTimeOffset? CreatedDt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ModifiedDt { get; set; } = DateTime.UtcNow;
    public CollateralLine? CollateralLine { get; set; }
    public LoanRequest? LoanRequest { get; set; }
}
