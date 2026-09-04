namespace Application.Features.Transactions.ForiegnExchanges;

public class ForeignExchangeDtoUpdate
{
    public long Id { get; set; }
    public Guid ActivityMainGroupId { get; set; }
    public string? StaffRefId { get; set; }
    public string? TranRefId { get; set; }
    public string? NameRefId { get; set; }
    public string? StaffAuthRefId { get; set; }
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? BuyCurrencyRefId { get; set; }
    public decimal? BuyAmount { get; set; }
    public int? BuyLedgerRefId { get; set; }
    public string? BuyCustAccRefId { get; set; }
    public int? SellCurrencyRefId { get; set; }
    public decimal? SellAmount { get; set; }
    public int? SellLedgerRefId { get; set; }
    public string? SellCustAccRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? SpotRate { get; set; }
    public int? GainAndLostLedgerRefId { get; set; }
    public decimal? GainAndLostAmount { get; set; }
    public int? FXEPALedgerRefId { get; set; }
    public int? FXPALedgerRefId { get; set; }
    public string? Narration { get; set; }
    public string? Remark { get; set; }
    public bool? Ispl { get; set; }
    public bool IsBusy { get; set; } = false;
    public decimal? FXEPA_Amount { get; set; } = 0;
    public decimal? FXPA_Amount { get; set; } = 0;
    public TransactionBase? Transaction { get; set; }
}
