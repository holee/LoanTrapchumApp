namespace Application.Features.CollateralValuations.Dtos
{
    public class CollateralValuationDtoCreate
    {
        public long? CollateralLineRefId { get; set; }
        public string? CollateralRefId { get; set; }
        public DateOnly? ValueDate { get; set; }
        public string? ValueBy { get; set; }
        public decimal? ValueAmount { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CreatedBy { get; set; }
    }
}
