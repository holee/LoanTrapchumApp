using Applications.Requests.Lists.Currency;

namespace Applications.Responses.Lists.Currency;

public class CurrencyResponse
{
    public int CurrencyId { get; set; }
    public string? CurrencyCode { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal? ExchangeRate { get; set; }
    public decimal? SpotRate { get; set; }
    public string? CurrencyType { get; set; } = string.Empty;
    public decimal? Limited { get; set; }
    public bool IsBaseCurrency { get; set; }=false;
    public bool IsActive { get; set; } = false; 
    public List<CurrencyDtoSpotRate>? SpotRates { get; set; } 

}
