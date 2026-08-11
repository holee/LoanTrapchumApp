namespace Applications.Features.Journals.Dtos;

public sealed class JournalLineDtoUpdate 
{
    public int JournalLineId { get; set; }
    public string? JournalRefId { get; set; }
    public string? EntityNameRefId { get; set; }
    public string? EntityName { get; set; } = string.Empty;
    public int? LedgerRefId { get; set; }
    public string? LedgerName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public decimal? Debit { get;set;}
    public decimal? Credit { get; set; }
    public decimal? ExchRate { get; set; }
    public decimal? BaseAmount { get; set; }
    public string? JournalLineDesc { get; set; }
}
