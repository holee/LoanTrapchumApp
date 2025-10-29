namespace Applications.Features.Collaterals.Dtos;

public sealed class CollateralLineDtoCreate
{
    public long Id { get; set; }
    public string? CollateralRefId { get; set; }
    public string? CustomerName { get; set; }
    public int? CategoryRefId { get; set; }
    public string? TypeName { get; set; }
    public int? SubCategoryRefId { get; set; }
    public string? SubTypeName { get; set; }
    public string? ReferenceNo { get; set; }
    public string? CollateralNo { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsReturn { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? OwnerName { get; set; }
    public string? Location { get; set; }
    public string? WareHouse { get; set; }
    public int? Quantity { get; set; }
    public string? Size { get; set; }
    public string? Description { get; set; }
    public decimal? MarketPrice { get; set; } = 0.00M;
    public decimal? HotPrice { get; set; } = 0.00M;
    public decimal? PawnPrice { get; set; } = 0.00M;
    public string? CollateralSubTypeName { get; set; }
    public bool IsUsed { get; set; } = false;
}
