namespace Applications.Features.Transactions.Journals
{
    public class JournalLineDto
    {
        public int JournalLineId { get; set; } 
        public long? JournalRefId { get; set; }
        public int? LineSeq { get; set; } = 0;
        public long? EntityNameRefId { get; set; }
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
}
