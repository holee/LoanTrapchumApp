namespace Applications.Requests.Transactions.Journals
{
    public class TrnxJournalDtoCreate
    {
        public int? TranTypeRefId { get; set; }
        public string TransactionTypeName { get; set; } = string.Empty;
        public long? UserRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public JournalDtoCreate JournalEntry { get; set; } = new();
    }
}
