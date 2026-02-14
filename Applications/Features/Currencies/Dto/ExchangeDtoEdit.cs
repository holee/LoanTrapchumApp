namespace Applications.Features.Currencies.Dto;

public sealed class ExchangeDtoEdit : ExchangeDtoCreate
{
    public long Id { get; set; } = 0;
    public string? TransactionId { get; set; }
    public DateTimeOffset? TransactionDate { get; set; }
}
