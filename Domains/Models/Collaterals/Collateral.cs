using Domains.Models.Lists.Customers;

namespace Domains.Models.Collaterals;


public sealed class Collateral
{
    public long? CollateralId { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? CustomerName { get; set; }
    public string? DocumentKeeping { get; set; } = string.Empty;
    public string? Remark { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public List<CollateralLine> CollateralLines { get; set; } = [];
}
