namespace Applications.Features.Customers.Queries;

public sealed class VCustomerCollateralLine
{
    public long Id { get; set; }
    public string CollateralId { get; set; } = default!;
    public DateTime? RegisterDate { get; set; }
    public string? Remark { get; set; }
    public string? CollateralNo { get; set; }
    public string? OwnerName { get; set; }
    public string? Location { get; set; }
    public string? ReferenceNo { get; set; }
    public decimal? Quantity { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? Description { get; set; }
    public string? Size { get; set; }
    public string? WareHouse { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsReturn { get; set; }
    public decimal? MarketPrice { get; set; }
    public decimal? PawnPrice { get; set; }
    public decimal? HotPrice { get; set; }
    public long? CategoryRefId { get; set; }
    public string? TypeName { get; set; }
    public long? SubCategoryRefId { get; set; }
    public string? CollateralSubTypeName { get; set; }
    public decimal ValueRate { get; set; } = 0.00M;
    public bool IsUsed { get; set; }
}
