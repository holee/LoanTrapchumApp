namespace Applications.Features.Currencies.Responses;

public sealed class VCurrencyCounter 
{
    public int Id { get; set; }
    public int? CurrencyRefId { get; set; }
    public string? CurrencyCode { get; set; } = string.Empty;
    public string? CurrencyName { get; set; } = string.Empty;
    public decimal? CurrencyValue { get; set; }
    public int? Quantity { get; set; }
    public string? Description { get; set; } = string.Empty; 
    public bool IsActive { get; set; } = true;
}
