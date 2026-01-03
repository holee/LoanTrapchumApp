namespace Applications.Features.Currencies.Requests;

public sealed class ExchangeDtoEdit : ExchangeDtoCreate
{
    public string? TransactionId { get; set; }
    public DateTimeOffset? TransactionDate { get; set; }
    public string? Remark { get; internal set; }
}
