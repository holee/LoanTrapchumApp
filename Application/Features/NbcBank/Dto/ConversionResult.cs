namespace Application.Features.NbcBank.Dto;

public sealed class ConversionResult
{
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public decimal OriginalAmount { get; set; }
    public decimal ConvertedAmount { get; set; }
    public decimal ExchangeRateUsed { get; set; }
    public DateTime AsOfDate { get; set; }
    public string Disclaimer { get; set; } = "Exchange rates are provided by the National Bank of Cambodia (NBC).";
}
