using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Currencies
{
    public class CurrencyDtoSpotRateEdit
    {
        public long SpotRateId { get; set; }
        public int CurrencyRefId { get; set; }

        [Required]
        public decimal? BuyingRate { get; set; }
        [Required]
        public decimal? SellingRate { get; set; }
        [Required]
        public decimal? SpotRate { get; set; }
        [Required]
        public DateTime? TransactionDate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Description { get; set; } = string.Empty;
    }


}
