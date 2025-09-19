namespace Applications.Features.Collaterals.Requests
{
    public class CollateralEvalutaionsDtoCreate
    {
        public long? CollateralRefId { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? ValueBy { get; set; }
        public decimal? ValueAmount { get; set; }
        public int CurrencyRefid { get; set; }
    }
}
