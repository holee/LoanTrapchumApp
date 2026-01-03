using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Currencies.Responses
{
    public sealed class VttCurrencyExchanges
    {

        public string? TransactionsId { get; set; }
        public string? TransactionTypeName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? UserRefId { get; set; }
        public long Id { get; set; }
        public int? BankRefId { get; set; }
        public int? BuyCurrencyRefId { get; set; }
        public string? BuyCurrencyName { get; set; }
        public decimal? BuyAmount { get; set; }
        public int? SellCurrencyRefId { get; set; }
        public string? SellCurrencyName { get; set; }
        public decimal? SellAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? SpotRate { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? Remark { get; set; }


        [NotMapped] public int RowIndex { get; set; }

        [NotMapped]
        public bool IsEditable
        {
            get
            {
                if (TransactionDate == null)
                    return false;
                return TransactionDate.Value.Date == DateTime.UtcNow.Date;
            }
        }

        public static Pager<VttCurrencyExchanges> Create(List<VttCurrencyExchanges> items, int counts = 0)
        {
            return new Pager<VttCurrencyExchanges>
            {
                Items = items,
                Count = counts
            };
        }
    }
}

