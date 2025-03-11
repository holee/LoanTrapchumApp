namespace Applications.Requests.Transactions.Journals
{
    public class JournalLineDtoCreate
    {
        public long? EntityNameRefId { get; set; }
        public string? EntityName { get; set; } = string.Empty;
        public int? LedgerRefId { get; set; }
        public string? LedgerName { get; set; } = string.Empty;
        public int LineSeq { get; set; } = 1;
        public decimal? Amount { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? BaseAmount { get; set; }
        public string? JournalLineDesc { get; set; }

        public bool IsDebit => Debit > 0;
        public bool IsCredit => Credit > 0;
    }
}
