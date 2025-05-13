namespace Domains.Models.Transaction
{
    public sealed class CashManagement
    {

        public string Id { get; set; } = null!;
        public int? BankRefId { get; set; } 
        public string? StaffRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public int? BranchRefId { get; set; }
        public string? TransactionRefId { get; set; }
        public string? EmployeeRefId { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public int? CashStatusRefId { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public DateTimeOffset? BusinessDate { get; set; }
        public string? Reject { get; set; } = "N";
        public string? Receive { get; set; } = "Y";
        public string? Remark { get; set; }
        public decimal? TmpAmount { get; set; }
        public long? DelegateToStaffRefId { get; set; }
        public List<CashManagementDetail>? CashManagementDetails { get; set; } = [];
    }
}
