namespace Application.Features.Journals.Dtos;

public sealed class JournalEntryDtoCreate
{

    public string? JournalEntryId { get; set; }
    public string? TransactionsRefId { get; set; } = string.Empty;
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; } = DateTime.UtcNow;
    public bool IsAdjustment { get; set; } = false;
    public string? ReferrenceNum { get; set; } = string.Empty;
    public string JournalDesc { get; set; } = string.Empty;

    public int JournalLineCount
    {
        get
        {
            return JournalEntryLines.Where(x => x.LedgerRefId is not null && x.LedgerRefId > 0 && (x.Debit > 0 || x.Credit > 0)).Count();
        }
    }

    public int? CurrencyRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr => JournalEntryLines.Sum(l => l.Debit) ?? 0;
    public decimal? TotalCr => JournalEntryLines.Sum(l => l.Credit) ?? 0;
    public List<JournalLineDtoCreate> JournalEntryLines { get; set; } = new List<JournalLineDtoCreate>();
    public decimal Difference => TotalDr.GetValueOrDefault() - TotalCr.GetValueOrDefault();
    public bool IsBalanced => Difference == 0 && TotalDr > 0;
}
