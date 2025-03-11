namespace Domains.Models.Collaterals;

public sealed class CollateralLinkDtoCreate 
{
    public long? Id { get; set; }
    public int? BranchId { get; set; }
    public long? LoanRequestRefId { get; set; }
    public long? CollateralRefId { get; set; }
    public long? CollateralLineRefId { get; set; }
    public bool? IsActive { get; set; } = true;

}
