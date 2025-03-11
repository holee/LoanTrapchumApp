using System.ComponentModel.DataAnnotations;

namespace Applications.Responses.ViewModels.Journals
{
    public class JournalEntryVm
    {

        public long? TranId { get; set; }

        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime? TranxDate { get; set; }
        public string? TransactionType { get; set; } = string.Empty;
        public string? JournalType { get; set; } = string.Empty;
        public string? CurrencyCode { get; set; } = string.Empty;
        public decimal? TotalDr { get; set; }
        public decimal? TotalCr { get; set; }
        public string? ReferrenceNum { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsAdjustment { get; set; } = false;
    }
}
