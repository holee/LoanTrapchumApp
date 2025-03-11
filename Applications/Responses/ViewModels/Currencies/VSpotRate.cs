namespace Applications.Responses.ViewModels.Currencies
{
    public class VSpotRate
    {
        public long SpotRateId { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CurrencyName { get; set; }
        public decimal? BuyingRate { get; set; }
        public decimal? SellingRate { get; set; }
        public decimal? SpotRate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Description { get; set; }
        public bool? Used { get; set; } = false;
    }
}
