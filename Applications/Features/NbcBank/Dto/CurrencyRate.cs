namespace Applications.Features.NbcBank.Dto;

public sealed class CurrencyRate
{
    public string CurrencyName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;
    public string QuoteCurrency { get; set; } = "KHR";
    public decimal Unit { get; set; } = 1;
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
    public decimal Average { get; set; }
    public decimal RatePerOneUnit => Unit > 0 ? Math.Round(Average / Unit, 6) : Average;
}
