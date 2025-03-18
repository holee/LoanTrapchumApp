namespace Domains.Models.Journals;

public sealed class JournalEntryLine
{
    public int JournalLineId { get; set; }
    public Guid? JournalRefId { get; set; }
    public long? LoanRefId { get; set; }
    public long? EntityNameRefId { get; set; }
    public string? EntityName { get; set; } = string.Empty;
    public int LineSeq { get; set; } = 1;
    public int? LedgerRefId { get; set; }
    public string LedgerName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? BaseAmount { get; set; }
    public string? JournalLineDesc { get; set; }
}
