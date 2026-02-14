namespace Applications.Features.Journals.Dtos;

public sealed class JournalEntryDto
{
    public string? JournalEntryId { get; set; }
    public string? TransactionsRefId { get; set; }
    public Guid? JournalCode { get; set; } = Guid.NewGuid();
    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; }
    public string JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; }
    public bool? IsAdjustment { get; set; } = false;
    public string? ReferrenceNum { get; set; } = string.Empty;
    public int? CurrencyRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr { get; set; } = 0;
    public decimal? TotalCr { get; set; } = 0;
    public decimal? TotalBaseDr { get; set; } = 0;
    public decimal? TotalBaseCr { get; set; } = 0;
    public int? JournalLineCount { get; set; } = 1;
    public string JournalDesc { get; set; } = string.Empty;
    public int TransactionsId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? Description { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public DateTimeOffset? ModifiedDt { get; set; }
    public ICollection<JournalEntryLineDto> JournalEntryLines { get; set; } = [];
}
