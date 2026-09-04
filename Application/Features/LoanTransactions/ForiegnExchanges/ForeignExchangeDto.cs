namespace Application.Features.Transactions.ForiegnExchanges;

public class ForeignExchangeDto
{
    public int Id { get; set; }
    public Guid ActivityMainGroupId { get; set; }
    public long? StaffRefId { get; set; }
    public long? TranRefId { get; set; }
    public long? NameRefId { get; set; }
    public long? StaffAuthRefId { get; set; }
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? BuyCurrencyRefId { get; set; }
    public decimal? BuyAmount { get; set; }
    public int? BuyLedgerRefId { get; set; }
    public Guid BuyCustAccRefId { get; set; }
    public int? SellCurrencyRefId { get; set; }
    public decimal? SellAmount { get; set; }
    public int? SellLedgerRefId { get; set; }
    public Guid SellCustAccRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? SpotRate { get; set; }
    public decimal? GainAndLost { get; set; }
    public int? GainAndLostLedgerRefId { get; set; }
    public decimal? GainAndLostAmount { get; set; }
    public int? FxepaLedgerRefId { get; set; }
    public decimal? FxepaAmount { get; set; }
    public int? FxpaLedgerRefId { get; set; }
    public decimal? FxpaAmount { get; set; }  
    public string? Narration { get; set; }
    public string? Remark { get; set; }
    public bool? Ispl { get; set; }
}
