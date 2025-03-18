namespace Domains.Models.Transaction;

public class CashVault
{
    public Guid CashVaultsId { get; set; } 
    public int CvId { get; set; }
    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; }
    public long? TranRefId { get; set; }
    public Guid? TransactionRefId { get; set; }
    public long? UserRefId { get; set; }
    public int? CvStatusRefId { get; set; }
    public DateTime? CvDate { get; set; }
}
