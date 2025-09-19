using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Transaction
{
    public sealed class CashManagementDetail
    {
        public long Id { get; set; }
        public string? CachmanagementRefId { get; set; } 
        public string? TransactionsRefId { get; set; }
        public int? BankRefId { get; set; }
        public int? BranchRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CurrencyRefName { get; set; }
        public int? CurrencyNoteRefId { get; set; }
        public int? CurrencyNoteValue { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [NotMapped]
        public decimal? TotalValue { get; set; }
    }
}
