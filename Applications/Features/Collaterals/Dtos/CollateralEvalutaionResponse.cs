namespace Applications.Features.Collaterals.Dtos
{
    public class CollateralEvalutaionResponse
    {
        public long Id { get; set; }
        public long? CollateralRefId { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? ValueBy { get; set; }
        public decimal? ValueAmount { get; set; }
        public int CurrencyRefid { get; set; }
    }
}
