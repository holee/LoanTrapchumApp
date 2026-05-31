using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Collaterals.Dtos;

public sealed class CollateralLineDtoCreate
{
    public CollateralLineDtoCreate()
    {

    }
    public CollateralLineDtoCreate(string? collateralId) : this()
    {
        CollateralRefId = collateralId;
    }
    public long Id { get; set; }
    public string? CollateralRefId { get; set; }
    public string? CustomerName { get; set; }
    public int? CategoryRefId { get; set; }
    public string? TypeName { get; set; }
    public int? SubCategoryRefId { get; set; }
    //public string? SubTypeName { get; set; }
    public string? ReferenceNo { get; set; }
    public string? CollateralNo { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsReturn { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? OwnerName { get; set; }
    public string? Location { get; set; }
    public string? WareHouse { get; set; }
    public int? Quantity { get; set; } = 1;
    public string? Size { get; set; }
    public string? Description { get; set; }

    [Required(ErrorMessage = "Market Price is required.")]
    public decimal? MarketPrice { get; set; } = 0.00M;

    [Required(ErrorMessage = "Hot Price is required.")]
    public decimal? HotPrice { get; set; } = 0.00M;

    [Required(ErrorMessage = "Pawn Price is required.")]
    public decimal? PawnPrice { get; set; } = 0.00M;
    public string? SubTypeName { get; set; }
    public string? FileUrl { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
}
