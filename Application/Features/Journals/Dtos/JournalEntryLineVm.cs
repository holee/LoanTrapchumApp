namespace Application.Features.Journals.Dtos;

public sealed class JournalEntryLineVm
{
    public long JournalLineId { get; set; }
    public string? JournalRefId { get; set; } // Foreign key to JournalEntry
    public string? LoanRefId { get; set; }
    public string? EntityName { get; set; }
    public int LineSeq { get; set; }
    public int? LedgerRefId { get; set; }
    public string? LedgerName { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal Amount { get; set; } = 0;
    public decimal? BaseAmount { get; set; }
    public string? JournalLineDesc { get; set; }
}