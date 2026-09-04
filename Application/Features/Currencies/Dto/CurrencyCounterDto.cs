namespace Application.Features.Currencies.Dto;

public sealed class CurrencyCounterDto
{
    public int Id { get; set; }
    public decimal? CurrencyValue { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public bool IsActive { get; set; } = true;
}
