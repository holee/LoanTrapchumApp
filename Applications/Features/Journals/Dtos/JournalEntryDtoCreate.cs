using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Journals.Dtos;

public sealed class JournalEntryDtoCreate
{

    public string? JournalEntryId { get; set; }
    public string? TransactionsRefId { get; set; }=string.Empty;
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; } = DateTime.UtcNow;
    public bool IsAdjustment { get; set; } = false;
    public string? ReferrenceNum { get; set; } = string.Empty; 
    public string JournalDesc { get; set; } = string.Empty;
    public int JournalLineCount { get; set; } = 1;
    public int? CurrencyRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr { get; set; }
    public decimal? TotalCr { get; set; }
    public List<JournalLineDtoCreate> JournalEntryLines { get; set; } = []; 
}
