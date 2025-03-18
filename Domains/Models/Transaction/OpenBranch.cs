namespace Domains.Models.Transaction
{
    public class OpenBranch
    {
        public long OpenBranchId { get; set; }
        public long? TranRefId { get; set; }
        public Guid? TransactionRefId { get; set; }
        public int? BankRefId { get; set; } 
        public int? BranchRefId { get; set; }
        public long? LoginLogId { get; set; }
        public DateTime? OpenDate { get; set; }
        public string? OpenStatus { get; set; }
        public long? DelegateStaffId { get; set; } 
        public string? IsContinue { get; set; }
    }
}
