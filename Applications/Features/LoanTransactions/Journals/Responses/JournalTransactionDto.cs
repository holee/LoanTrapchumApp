namespace Applications.Features.Transactions.Journals;

public class JournalTransactionDto 
{
    public long? TranId { get; set; } 
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public long? UserRefId { get; set; }
    public int? CurrencyRefId { get; set; }
}
