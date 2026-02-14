namespace Applications.Features.Currencies.Dto;

public sealed class CurrencyCounterDtoUpdate 
{
    public int Id { get; set; }
    public int? CurrencyRefId { get; set; }
    public decimal? CurrencyValue { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public bool IsActive { get; set; } = true;
}
