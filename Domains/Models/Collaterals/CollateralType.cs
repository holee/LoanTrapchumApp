namespace Domains.Models.Collaterals;

public class CollateralType
{
    public int CollateralTypeId { get; set; }
    public string? CollateralTypeCode { get; set; }
    public bool IsActive { get; set; } = false;
    public string? Description { get; set; }
}
