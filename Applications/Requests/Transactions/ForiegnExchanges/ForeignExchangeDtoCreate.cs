namespace Applications.Requests.Transactions.ForiegnExchanges;

public sealed class ForeignExchangeDtoCreate 
{
    public long? StaffRefId { get; set; }
    public long? TranRefId { get; set; }
    public long? NameRefId { get; set; }
    public long? StaffAuthRefId { get; set; }
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? BuyCurrencyRefId { get; set; }
    public decimal? BuyAmount { get; set; }
    public int? BuyLedgerRefId { get; set; }
    public int? SellCurrencyRefId { get; set; }
    public decimal? SellAmount { get; set; }
    public int? SellLedgerRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? SpotRate { get; set; }
    public int? GainAndLostLedgerRefId { get; set; }
    public decimal? GainAndLostAmount { get; set; }
    public int? FXEPA_LedgerRefId { get; set; }
    public decimal? FXEPA_Amount { get; set; }
    public int? FXPA_LedgerRefId { get; set; }
    public decimal? FXPA_Amount { get; set; }
    public string? Remark { get; set; }

}
