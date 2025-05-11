using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Journals
{
    public sealed class JournalEntryVm
    {
        public string? JournalEntryId { get; set; }
        public string? TransactionsId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? JournalType { get; set; }
        public string? ReferrenceNum { get; set; }
        public decimal? TotalDr { get; set; }
        public decimal? TotalCr { get; set; }
        public bool IsAdjustment { get; set; } = false;
        public DateTime? JournalDate { get; set; }
        
        public decimal? TotalBaseDr { get; set; }
        
        public decimal? TotalBaseCr { get; set; }
        
        public int? JournalLineCount { get; set; }
        
        public decimal? ExchangeRate { get; set; }
        
        public Guid? JournalCode { get; set; } 
        public string? Description { get; set; } // Alias for JournalDesc
        
        public int? BranchId { get; set; }
        
        public string? BranchName { get; set; }
        
        public int? CurrencyId { get; set; }
        
        public string? CurrencyCode { get; set; }
        
        [NotMapped]
        public ICollection<JournalEntryLineVm> JournalEntryLines { get; set; } = [];
    }
}
