using Applications.Features.Ledgers.Dto;

namespace Applications.Features.Currencies.Dto;

public sealed class CurrencyDto
{
    public int CurrencyId { get; set; }
    public string? CurrencyCode { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal? ExchangeRate { get; set; }
    public decimal? SpotRate { get; set; }
    public string? CurrencyType { get; set; } = string.Empty;
    public decimal? Limited { get; set; }
    public bool IsBaseCurrency { get; set; } = false;
    public bool IsActive { get; set; } = false;

    public ICollection<CurrencySpotRateDto> CurrencySpotRates { get; set; } = [];
    public ICollection<CurrencyCounterDto> CurrencyCounters { get; set; } = [];
    public ICollection<LedgerDto> Ledgers { get; set; } = [];
}
