using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Transactions.Journals;

public sealed class JournalEntryDtoCreate
{

    public string? JournalEntryId { get; set; }
    public string? TransactionsRefId { get; set; }=string.Empty;
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }


    [Required(ErrorMessage = "Journal type is required.")]
    public string JournalType { get; set; } = string.Empty;
    public DateTime? JournalDate { get; set; } = DateTime.UtcNow;
    public bool IsAdjustment { get; set; } = false;

    [Required(ErrorMessage = "Reference number is required.")]
    [StringLength(20, ErrorMessage = "Reference number cannot exceed 20 characters.")]
    public string ReferrenceNum { get; set; } = string.Empty; 
    public string JournalDesc { get; set; } = string.Empty;
    public int JournalLineCount { get; set; } = 1;

    [Required(ErrorMessage = "Currency reference ID is required.")]
    public int? CurrencyRefId { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? TotalDr { get; set; }
    public decimal? TotalCr { get; set; }

    [ValidateComplexType]
    public List<JournalLineDtoCreate> JournalEntryLines { get; set; } = []; 
}
