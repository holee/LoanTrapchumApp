using Domains.Models.Bases;

namespace Domains.Models.Transaction
{
    public sealed class ForeignExchange :BaseEntity
    {
        public long Id { get; set; }
        public Guid? ActivityMainGroupId { get; set; }
        public string? StaffRefId { get; set; }
        public string? TransactionRefId { get; set; }
        public string? NameRefId { get; set; }
        public int? BankRefId { get; set; }
        public int? BranchRefId { get; set; }   
        public string? StaffAuthRefId { get; set; }
        public int? BuyCurrencyRefId { get; set; }
        public decimal? BuyAmount { get; set; }
        public int? BuyLedgerRefId { get; set; }
        public Guid? BuyCustAccRefId { get; set; }
        public int? SellCurrencyRefId { get; set; }
        public decimal? SellAmount { get; set; }
        public int? SellLedgerRefId { get; set; }
        public Guid? SellCustAccRefId { get; set; }
        public decimal? SpotRate { get; set; }  
        public decimal? ExchangeRate { get; set; }
        public int? GainAndLostLedgerRefId { get; set; }
        public decimal? GainAndLostAmount { get; set; }
        public int? FxepaLedgerRefId { get; set; }
        public decimal? FXEPA_Amount { get; set; }
        public int? FxpaLedgerRefId { get; set; }
        public decimal? FXPA_Amount { get; set; }
        public string? Narration { get; set; }
        public string? Remark { get; set; }
        public bool? Ispl { get; set; }
    }
}

