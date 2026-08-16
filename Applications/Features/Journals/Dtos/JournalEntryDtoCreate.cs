namespace Applications.Features.Journals.Dtos;

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
    public decimal? TotalDr { get; set; }
    public decimal? TotalCr { get; set; }
    public List<JournalLineDtoCreate> JournalEntryLines { get; set; } = [];
}
