namespace Applications.Requests.Transactions.Journals;

public class JournalEntryDto
{
    public long? JournalId { get; set; }
    public string JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; } = DateTime.UtcNow;
    public bool IsAdjustment { get; set; } = false;
    public string ReferrenceNum { get; set; } = string.Empty;
    public string JournalDesc { get; set; } = string.Empty;
    public int? CurrencyRefId { get; set; }
    public decimal? TotalDr { get; set; }
    public decimal? TotalCr { get; set; }
    public List<JournalLineDto>? JournalEntryLines { get; set; }
    public JournalTransactionDto? LoanTransaction { get; set; } 
}
