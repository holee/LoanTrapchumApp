namespace Applications.Features.Journals.Dtos;

public sealed class JournalEntryDtoUpdate
{
    public string? JournalId { get; set; }
    public string? JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; } = DateTime.UtcNow;
    public bool IsAdjustment { get; set; } = false;
    public string? ReferrenceNum { get; set; } = string.Empty;
    public string? JournalDesc { get; set; } = string.Empty;
    public int? CurrencyRefId { get; set; }
    public string? JournalEntryId { get; set; }
    public string? TransactionsRefId { get; set; } = string.Empty;
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int JournalLineCount
    {
        get
        {
            return JournalEntryLines?.Where(x => x.LedgerRefId is not null && x.LedgerRefId > 0 && (x.Debit > 0 || x.Credit > 0)).Count() ?? 0;
        }
    }

    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr => JournalEntryLines?.Sum(l => l.Debit) ?? 0;
    public decimal? TotalCr => JournalEntryLines?.Sum(l => l.Credit) ?? 0;
    public decimal Difference => TotalDr.GetValueOrDefault() - TotalCr.GetValueOrDefault();
    public bool IsBalanced => Difference == 0 && TotalDr > 0;
    public List<JournalLineDtoUpdate>? JournalEntryLines { get; set; } = new();

}
