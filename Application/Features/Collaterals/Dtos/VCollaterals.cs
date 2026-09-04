using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Collaterals.Dtos;

public sealed class VCollateral
{
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? FullName { get; set; } 
    public DateTime? RegisterDate { get; set; }
    public string? Remark { get; set; }
    [NotMapped]
    public ICollection<VCollateralLines> CollateralLines { get; set; } = [];
}
