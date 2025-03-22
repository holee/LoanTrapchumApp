using Domains.Models.Bases;

namespace Domains.Models.Currencies
{
    public class CurrencySpotRate : BaseEntity
    {
        public long SpotRateId { get; set; }
        public int CurrencyRefId { get; set; }
        public decimal? BuyingRate { get; set; }
        public decimal? SellingRate { get; set; }
        public decimal? SpotRate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public Currency?  Currency { get; set; }
    }
}
