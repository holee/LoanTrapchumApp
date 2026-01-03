namespace Applications.Features.Currencies.Requests;

public class ExchangeDtoCreate
{
    public int CurrencyTypeId { get; set; }
    public int SellCurrencyId { get; set; }
    public int BuyCurrencyId { get; set; }
    public decimal? Amount { get; set; } = 0;
    public decimal? ExchangeRate { get; set; } = 0;
    public decimal? TotalAmount { get; set; } = 0;
    public decimal? SpotRate { get; set; } = 0;
    public string? Description { get; set; }
    public string? ExchangedMode { get; set; } = "Sell Currency";
    public decimal? BuyAmount { get; set; } = 0;
    public decimal? SellAmount { get; set; } = 0;
}
