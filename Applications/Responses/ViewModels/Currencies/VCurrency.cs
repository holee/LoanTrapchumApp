namespace Applications.Responses.ViewModels.Currencies
{
    public class VCurrencyList
    {
        public int CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Description { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? SpotRate { get; set; }
        public string? CurrencyType { get; set; }
        public decimal? Limited { get; set; }
        public bool? IsBaseCurrency { get; set; } = false;
        public bool? IsActive { get; set; } = true;
    }
}
