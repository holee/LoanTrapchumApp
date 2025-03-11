using Domains.Models.Currencies;

namespace Domains.Models.Journals;

public class Ledger
{
    public int LedgerId { get; set; }
    public string? LedgerCode { get; set; }
    public string? LedgerName { get; set; }
    public string? Description { get; set; }
    public bool? IsMainGl { get; set; }
    public int? ParentRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public string? BankNumber { get; set; }
    public int? LedgerTypeRefId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsConsolidate { get; set; }
    public Currency? Currency { get; set; }
    public LedgerType? LedgerType { get; set; }
    public Ledger?  ParentLedger { get; set; } 
}