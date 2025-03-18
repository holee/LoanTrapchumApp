using Domains.Models.Transaction;

namespace Domains.Models.Journals;

public class JournalEntry:BaseEntity
{
    public long JournalId { get; set; }
    public Guid JournalEntryId { get; set; }
    public Guid? TransactionsRefId { get; set; }
    public Guid? JournalCode { get; set; }=Guid.NewGuid();
    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; } 
    public string JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; }
    public bool IsAdjustment { get; set; } = false;
    public string? ReferrenceNum { get; set; } = string.Empty;
    public int? CurrencyRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr { get; set; } = 0;
    public decimal? TotalCr { get; set; } = 0;
    public decimal? TotalBaseDr { get; set; } = 0;
    public decimal? TotalBaseCr { get; set; } = 0;
    public int JournalLineCount { get; set; } = 1;
    public string JournalDesc { get; set; } = string.Empty;
    public LoanTransaction? LoanTransaction { get; set; }
    public List<JournalEntryLine>? JournalEntryLines { get; set; } = [];
    
}
