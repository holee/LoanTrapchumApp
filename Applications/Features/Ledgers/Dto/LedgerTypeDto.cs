namespace Applications.Features.Ledgers.Dto;

public class LedgerTypeDto
{
    public int LedgerTypeId { get; set; }
    public string? LedgerTypeCode { get; set; }
    public string? CrOrDr { get; set; }
    public int NOrder { get; set; }
    public LedgerDto? LedgerDto { get; set; }    
}
