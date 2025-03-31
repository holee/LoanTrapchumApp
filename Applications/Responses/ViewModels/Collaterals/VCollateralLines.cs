namespace Applications.Responses.ViewModels.Collaterals;

public sealed class VCollateralLines
{
    public long? Id { get; set; }
    public string? CollateralId { get; set; }
    public DateTime? RegisterDate { get; set; } 
    public string? Remark { get; set; }
    public string? CollateralNo { get; set; }
    public string? OwnerName { get; set; }
    public string? Location { get; set; }
    public string? ReferenceNo { get; set; }
    public int? Quantity { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? Description { get; set; }
    public string? Size { get; set; }
    public string? TypeName { get; set; }
    public string? WareHouse { get; set; }
    public bool IsAvailable { get; set; } = false;
    public bool IsReturn { get; set; } = false;
    public decimal? MarketPrice { get; set; } = 0;
    public decimal? PawnPrice { get; set; } = 0;
    public decimal? HotPrice { get; set; } = 0;
    public string? CollateralSubTypeName { get; set; }
    public bool IsInUsed { get; set; } = false;
}
