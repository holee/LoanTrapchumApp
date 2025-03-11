namespace Domains.Models.Transaction
{
    public class CashVaultTransaction
    {
        public int CashVaultId { get; set; }
        public int BrachRefId { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
    }
}
