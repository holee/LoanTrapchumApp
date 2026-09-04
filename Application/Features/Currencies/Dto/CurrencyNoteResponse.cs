namespace Application.Features.Currencies.Dto;

public class CurrencyNoteResponse
{
    public int Id { get; set; }
    public int? CurrencyRefId { get; set; }
    public decimal? CurrencyValue { get; set; }
    public int? Quantity { get; set; } 
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}
