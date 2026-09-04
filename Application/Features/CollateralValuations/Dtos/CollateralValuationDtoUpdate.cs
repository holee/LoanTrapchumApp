namespace Application.Features.CollateralValuations.Dtos
{
    public class CollateralValuationDtoUpdate
    {
        public long Id { get; set; }
        public long? CollateralLineRefId { get; set; }
        public string? CollateralRefId { get; set; }
        public DateOnly? ValueDate { get; set; }
        public string? ValueBy { get; set; }
        public decimal? ValueAmount { get; set; }
        public int? CurrencyRefId { get; set; }
    }
}
