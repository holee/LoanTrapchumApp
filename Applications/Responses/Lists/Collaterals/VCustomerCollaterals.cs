using Applications.Extensions.mappers;

namespace Applications.Responses.Lists.Collaterals;

public sealed class VCustomerCollaterals
{
    public string? CollateralId { get; set; }
    public DateTime? RegisterDate { get; set; } = DateTime.Now;
    public string? CustomerName { get; set; }
    public string? DocumentKeeping { get; set; }
    public string? Remark { get; set; }
    public List<VCollateralLines> CollateralLines { get; set; } = [];

}
