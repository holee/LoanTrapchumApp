namespace Applications.Features.Journals.Dtos;

public class JournalTransactionDtoUpdate
{
    public string? TransactionsId { get; set; } 
    public int? TranTypeRefId { get; set; }
    public string TransactionName { get; set; } = string.Empty;
    public string? UserRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public DateTimeOffset? TransactionDate { get; set; }
    public int? BranchRefId { get; set; }
    public string? TransactionTypeName { get; internal set; }
    public JournalEntryDtoUpdate? JournalEntry { get; set; }
}
