namespace Domains.Models.Journals;

public sealed class OperationsJournalEntry
{
    public string OpJournalEntryId { get; set; }
    public string? TransactionsRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; }
    public string? JournalType { get; set; }
    public DateTime? JournalDate { get; set; }
    public bool? IsAdjustment { get; set; }
    public string? ReferrenceNum { get; set; }
    public int? CurrencyRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr { get; set; }
    public decimal? TotalCr { get; set; }
    public decimal? TotalBaseDr { get; set; }
    public decimal? TotalBaseCr { get; set; }
    public int? JournalLineCount { get; set; }
    public int? EditSeq { get; set; }
    public string? JournalDesc { get; set; }
    public DateTime? CreatedDt { get; set; }
    public DateTime? ModifiedDt { get; set; }
}