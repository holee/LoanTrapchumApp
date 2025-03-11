using Domains.Models.Journals;
using Domains.Models.Loans;

namespace Domains.Models.Currencies;

public class Currency:BaseEntity
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
    public void AddSpotRates(IEnumerable<CurrencySpotRate> spotRates)
    {
        SpotRates = new List<CurrencySpotRate>();
        SpotRates.AddRange(spotRates);
    }

    public void AddSpotRate(CurrencySpotRate spotRates) 
    {
        SpotRates = new List<CurrencySpotRate>();
        SpotRates.Add(spotRates);
    }

    public void AddCounters(IEnumerable<CurrencyCounter> counters) 
    {
        CurrencyCounters = new List<CurrencyCounter>();
        CurrencyCounters.AddRange(counters);
    }

    public void AddCounter(CurrencyCounter counter)
    {
        CurrencyCounters = new List<CurrencyCounter>();
        CurrencyCounters.Add(counter);
    }



}
