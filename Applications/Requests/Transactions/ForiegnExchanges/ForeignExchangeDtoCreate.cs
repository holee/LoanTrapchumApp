namespace Applications.Requests.Transactions.ForiegnExchanges;

public sealed class ForeignExchangeDtoCreate 
{
    public string? StaffRefId { get; set; }
    public string? TranRefId { get; set; }
    public string? NameRefId { get; set; }
    public string? StaffAuthRefId { get; set; }
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
    public int? FxepaLedgerRefId { get; set; }
    public decimal? FXEPA_Amount { get; set; }
    public int? FxpaLedgerRefId { get; set; }
    public decimal? FXPA_Amount { get; set; }
    public string? Remark { get; set; }

}
