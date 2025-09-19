using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Currencies
{
    public class CurrencyDtoCreateSpotRate
    {
        public int CurrencyRefId { get; set; }
        [Required(ErrorMessage ="Please provide Buying Rate")]
        public decimal? BuyingRate { get; set; }
        [Required(ErrorMessage = "Please provide Selling Rate")]
        public decimal? SellingRate { get; set; }
        [Required(ErrorMessage = "Please provide Spot Rate")]
        public decimal? SpotRate { get; set; }

        [Required(ErrorMessage = "Please provide Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime? TransactionDate { get; set; }
        public string? Description { get; set; } = string.Empty;
    }


}
