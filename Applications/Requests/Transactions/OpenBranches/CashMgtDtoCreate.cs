namespace Applications.Requests.Transactions.OpenBranches
{
    public class CashMgtDtoCreate 
    {
        public int? BankRefId { get; set; }
        public long? StaffRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public int? BranchRefId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public int? CashStatusRefId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string? Remark { get; set; }
    }
}
