using Domains.Models.Bases;
using Domains.Models.Journals;
using Domains.Models.Loans;

namespace Domains.Models.Currencies;

public sealed class Currency:BaseEntity
{

    public int CurrencyId { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal? ExchangeRate { get; set; }
    public decimal? SpotRate { get; set; }
    public string? CurrencyType { get; set; } = string.Empty;   
    public decimal? Limited { get; set; }
    public bool? IsBaseCurrency { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Ledger>? Ledgers { get; set; }
    public List<CurrencySpotRate>? SpotRates { get; set; }
    public List<CurrencyCounter>? CurrencyCounters { get; set; }
    public List<LoanProduct>? LoanProducts { get; set; }

    public void AddSpotRate(CurrencySpotRate spotRates) 
    {
        SpotRates = new List<CurrencySpotRate>();
        SpotRates.Add(spotRates);
    }

}
