namespace Applications.Features.Journals.Dtos;

public class JournalTransactionDtoUpdate
{
    public string? TranId { get; set; } 
    public int? TranTypeRefId { get; set; }
    public string TransactionName { get; set; } = string.Empty;
    public string? UserRefId { get; set; }
    public int? CurrencyRefId { get; set; }
}
