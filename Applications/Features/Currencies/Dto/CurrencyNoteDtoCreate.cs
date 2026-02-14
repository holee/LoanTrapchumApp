namespace Applications.Features.Currencies.Dto;

public class CurrencyNoteDtoCreate
{
    public int? CurrencyRefId { get; set; }
    public int? CurrencyValue { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
}