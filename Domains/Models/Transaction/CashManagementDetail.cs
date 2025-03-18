using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Transaction
{
    public class CashManagementDetail
    {
        public long CmdId { get; set; }
        public long? CmRefId { get; set; }
        public Guid? CashmanagmentRefId { get; set; } 
        public long? TranRefId { get; set; }
        public int? BankRefId { get; set; }
        public int? BranchRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CurrencyRefName { get; set; }
        public int? CurrencyNoteRefId { get; set; }
        public int? CurrencyNoteValue { get; set; }
        public int? Quantity { get; set; }
        [NotMapped]
        public decimal? TotalValue { get; set; }
    }
}
