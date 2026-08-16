namespace Applications.Features.Ledgers.Dto;

public sealed class LedgerDto
{
    public int LedgerId { get; set; }
    public string? LedgerCode { get; set; } = string.Empty;
    public string? LedgerName { get; set; } = string.Empty;
    public int? ParentRefId { get; set; }
    public string? CurrencyCode { get; set; } = string.Empty;
    public string? LedgerTypeName { get; set; } = string.Empty;
    public string? BankNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsConsolidate { get; set; } = false;
    public bool IsMainGl { get; set; } = false;
    public string? Description { get; set; }
    public string? DisplayLedgerName { get; set; }
    public int HierarchyLevel { get; set; }
    public string? LedgerPath { get; set; }
    public bool IsMainGL { get; set; }
    public string? ParentLedgerName { get; set; }
    public int LedgerTypeID { get; set; }
    public string? LedgerTypeCode { get; set; }
    public int? CurrencyId { get; set; }
    public ICollection<JournalLineDto> JournalEntryLines { get; set; } = [];

}
