namespace Applications.Features.Ledgers.Dto;

public class LedgerViewModel
{
    public int LedgerId { get; set; }
    public string LedgerCode { get; set; } = string.Empty;
    public string LedgerName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string LedgerType { get; set; } = string.Empty;
    public string CrOrDr { get; set; } = string.Empty;
    public bool? IsMainGl { get; set; }
    public bool? IsActive { get; set; }
}
