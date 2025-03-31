using Domains.Models.Loans;
namespace Domains.Models.Collaterals;

public sealed class CollateralLink
{

    public long Id { get; set; }
    public int? BranchId { get; set; }
    public long? CollateralLineRefId { get; set; }
    public Guid? LoanRequestRefId { get; set; }
    public Guid? CollateralRefId { get; set; }
    public bool? IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
    public DateTime? CreatedDt { get; set; } = DateTime.Now;
    public DateTime? ModifiedDt { get; set; } = DateTime.Now;
    public CollateralLine? CollateralLine { get; set; }
    public LoanRequest? LoanRequest { get; set; }
}
