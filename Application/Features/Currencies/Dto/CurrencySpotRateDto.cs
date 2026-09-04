namespace Application.Features.Currencies.Dto;

public sealed class CurrencySpotRateDto
{
    public long SpotRateId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public decimal? BuyingRate { get; set; }
    public decimal? SellingRate { get; set; }
    public decimal? SpotRate { get; set; }
    public decimal? AvgRate { get; set; }
    public string? Description { get; set; } = string.Empty;
}