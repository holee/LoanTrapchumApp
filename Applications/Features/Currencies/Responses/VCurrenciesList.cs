namespace Applications.Features.Currencies.Responses;

public class VCurrenciesList
{
    public int CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? BuyingRate { get; set; }
    public decimal? SellingRate { get; set; }
    public decimal? SpotRate { get; set; }
    public bool IsBaseCurrency { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime? AsOfDate { get; set; }
}
