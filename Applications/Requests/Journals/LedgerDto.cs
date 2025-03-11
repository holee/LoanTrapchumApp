using Applications.Requests.Lists.Currency;
using Applications.Requests.Transactions.Journals;
namespace Applications.Requests.Journals;

public sealed class LedgerDto
{
    public int LedgerId { get; set; }
    public string? LedgerCode { get; set; } = string.Empty;
    public string? LedgerName { get; set; }=string.Empty;
    public int? ParentRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public int? LedgerTypeRefId { get; set; }
    public string? BankNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsConsolidate { get; set; } = false;
    public bool IsMainGl { get; set; } = false;
    public string? Description { get; set; }
    public LedgerTypeDto? LedgerType { get; set; }
    public CurrencyDto?   Currency { get; set; }
    public ICollection<JournalLineDto>? JournalEntryLines { get; set; } 
}
