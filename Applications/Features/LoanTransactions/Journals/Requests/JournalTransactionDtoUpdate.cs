namespace Applications.Features.Transactions.Journals;

public class JournalTransactionDtoUpdate
{
    public long TranId { get; set; } 
    public int? TranTypeRefId { get; set; }
    public string TransactionName { get; set; } = string.Empty;
    public long? UserRefId { get; set; }
    public int? CurrencyRefId { get; set; }
}
