namespace Applications.Features.NbcBank.Dto;

public sealed class NbcExchangeRateResponse
{
    public string Source { get; set; } = "National Bank of Cambodia (NBC)";
    public string SourceUrl { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public DateTime RetrievedAtUtc { get; set; } = DateTime.UtcNow;
    public string Language { get; set; } = "en";
    public OfficialExchangeRate OfficialRate { get; set; } = new();
    public List<CurrencyRate> Rates { get; set; } = new();
}
