using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Collaterals.Dtos; 


public sealed class CollateralDtoCreate
{
    public string? CollateralId { get; set; }
    public DateTime? RegisterDate { get; set; }

    [Required(ErrorMessage = "Customer Name Is Required.")]
    public string? CustomerName { get; set; }
    public string? DocumentKeeping { get; set; } = string.Empty;
    public string? Remark { get; set; } = string.Empty;

    public List<CollateralLineDtoCreate> CollateralLines { get; set; } = [];
}
