namespace Applications.Requests.Transactions.Journals;

public class JournalTransactionDtoCreate 
{
    public string? TransactionsId { get; set; }
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public string? UserRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public int? BranchRefId { get; set; } 
    public JournalEntryDtoCreate JournalEntry { get; set; } = new();

}
