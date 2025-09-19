namespace Domains.Models.Transaction;

public sealed class CashVault
{
    public string CashVaultsId { get; set; } =null!; 
    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; }
    public string? TransactionRefId { get; set; }
    public string? UserRefId { get; set; }
    public int? CvStatusRefId { get; set; }
    public DateTime? CvDate { get; set; }
}
