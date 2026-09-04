namespace Application.Features.Currencies.Dto;

public sealed class CurrencyNoteViewDto
{
    public int Id { get; set; }
    public int? CurrencyRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string CurrencyName { get; set; } = string.Empty;
    public decimal? CurrencyValue { get; set; } = 0;
    public string? Description { get; set; }
    public int Quantity { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
