namespace Applications.Features.Collaterals.Dtos
{
    public sealed class CollateralLineDtoUpdate
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
        public decimal? MarketPrice { get; set; }
        public decimal? HotPrice { get; set; }
        public decimal? PawnPrice { get; set; }

    }
}
