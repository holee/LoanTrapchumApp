using Applications.Responses.ViewModels.Currencies;

namespace Applications.Requests.Lists.Currency
{
    public sealed class ExchangeDtoCreate
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
        public VCurrenciesList? Currency { get; set; } = new();
        public decimal? BuyAmount { get; set; } = 0;
        public decimal? SellAmount { get; set; } = 0;
    }
}
