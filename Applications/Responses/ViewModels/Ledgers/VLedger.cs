namespace Applications.Responses.ViewModels.Ledgers;

public sealed class VLedger
{
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public string? LedgerCode { get; set; }
    public string? BankNumber { get; set; }
    public bool? IsMainGl { get; set; }
    public bool? IsConsolidate { get; set; }
    public string? Description { get; set; }
    public int? ParentRefId { get; set; }
    public string? ParentLedgerName { get; set; }
    public int? LedgerTypeId { get; set; }
    public string? LedgerTypeCode { get; set; }
    public bool? IsActive { get; set; }
    public int? CurrencyId { get; set; } 
    public string? CurrencyCode { get; set; }
}
