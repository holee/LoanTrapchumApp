namespace Applications.Features.CollateralValuations.Dtos
{
    public sealed class CollateralValuationDto
    {
        public long Id { get; set; }
        public long? CollateralLineRefId { get; set; }
        public string? CollateralRefId { get; set; }
        public DateOnly? ValueDate { get; set; }
        public string? ValueBy { get; set; }
        public decimal? ValueAmount { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDT { get; set; }
        public DateTimeOffset? ModifiedDT { get; set; }
    }
}
