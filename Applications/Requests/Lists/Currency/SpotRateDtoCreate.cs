using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Requests.Lists.Currency
{
    public sealed class SpotRateDtoCreate
    {
        public long SpotRateId { get; set; }
        public int CurrencyRefId { get; set; }
        public decimal? BuyingRate { get; set; }
        public decimal? SellingRate { get; set; }
        public decimal? SpotRate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
