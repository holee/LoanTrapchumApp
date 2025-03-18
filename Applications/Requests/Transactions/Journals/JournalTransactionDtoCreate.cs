namespace Applications.Requests.Transactions.Journals;

public class JournalTransactionDtoCreate 
{
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public string? UserRefId { get; set; }
    public int? CurrencyRefId { get; set; } 
}
