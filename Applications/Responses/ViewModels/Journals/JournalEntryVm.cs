namespace Applications.Responses.ViewModels.Journals
{
    public sealed class VJournal
    {
        public string? JournalEntryId { get; set; } 
        public string? TransactionsId { get; set; }
        public string? TransactionTypeName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? ReferrenceNum { get; set; }
        public string? JournalType { get; set; }
        public decimal? TotalCr { get; set; }
        public decimal? TotalDr { get; set; }
        public bool? IsAdjustment { get; set; }
        public string? Description { get; set; }
    }
}
