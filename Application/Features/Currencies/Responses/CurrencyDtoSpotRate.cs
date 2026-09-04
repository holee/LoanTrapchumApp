using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Currencies
{
    public class CurrencyDtoSpotRate 
    {
        public long SpotRateId { get; set; }
        public int CurrencyRefId { get; set; }
        public decimal? BuyingRate { get; set; }
        public decimal? SellingRate { get; set; }
        public decimal? SpotRate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int EditSeq { get; set; } = 0;
        [NotMapped]public int RowIndex { get; set; } = 0;
    }
}
