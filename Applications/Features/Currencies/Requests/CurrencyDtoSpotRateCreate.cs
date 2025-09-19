using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Currencies
{
    public class CurrencyDtoSpotRateCreate
    {
        [Required(ErrorMessage ="Please input buying rate.")]
        public decimal? BuyingRate { get; set; }
        [Required(ErrorMessage = "Please input selling rate.")]
        public decimal? SellingRate { get; set; }

        [Required(ErrorMessage = "Please input spot rate.")]
        public decimal? SpotRate { get; set; }
        [Required(ErrorMessage = "Please select the correct date.")]
        [DataType(DataType.Date)]
        public DateTime? TransactionDate { get; set; }
        public int EditSeq { get; set; } = 0;
    }
}
