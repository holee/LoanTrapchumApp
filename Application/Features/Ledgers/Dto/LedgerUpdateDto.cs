namespace Application.Features.Ledgers.Dto;

public sealed class LedgerUpdateDto
{

    public int LedgerId { get; set; }
    public string? LedgerCode { get; set; }
    public string? LedgerName { get; set; }
    public string? Description { get; set; }
    public bool IsMainGl { get; set; } = true;
    public int? ParentRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public string? BankNumber { get; set; } = string.Empty;
    public int? LedgerTypeRefId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsConsolidate { get; set; } = false;
    public  bool  IsSubTypeOf { get; set; } 
}
