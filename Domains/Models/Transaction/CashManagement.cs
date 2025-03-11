namespace Domains.Models.Transaction
{
    public class CashManagement
    {
        public long CashManagementId { get; set; }
        public int? BankRefId { get; set; } 
        public long? StaffRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public int? BranchRefId { get; set; }
        public long? TranRefId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public int? CashStatusRefId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string? Reject { get; set; } = "N";
        public string? Receive { get; set; } = "Y";
        public string? Remark { get; set; }
        public decimal? TmpAmount { get; set; }
        public long? DelegateToStaffRefId { get; set; }
        public List<CashManagementDetail>? CashManagementDetails { get; set; }
    }
}
