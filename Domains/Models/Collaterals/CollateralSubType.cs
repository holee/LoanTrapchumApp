using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Collaterals;

[Table("st_collateralSubTypes")]
public class CollateralSubType
{
    public int Id { get; set; }
    public string? CollateralSubTypeName { get; set; }
    public string? CollateralSubTypeNameInKhmer { get; set; }
    public int? CollateralTypeRefid { get; set; }
    public decimal? ValueRate { get; set; }
    public decimal? LoanSizeAmuntMin { get; set; }
    public decimal? LoanSizeAmuntMax { get; set; }
    public bool? IsActive { get; set; }
}
