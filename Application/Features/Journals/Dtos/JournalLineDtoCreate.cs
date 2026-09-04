using System.ComponentModel.DataAnnotations;

namespace Application.Features.Journals.Dtos;

public class JournalLineDtoCreate
{
    public string? EntityNameRefId { get; set; }
    public string? EntityName { get; set; } = string.Empty;
    public int? LedgerRefId { get; set; }
    public string LedgerName { get; set; } = string.Empty;
    public int LineSeq { get; set; } = 1;
    public decimal? Amount { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? BaseAmount { get; set; }
    public string? JournalLineDesc { get; set; }
    public bool IsDebit => Debit > 0;
    public bool IsCredit => Credit > 0;
    public long JournalLineId { get; set; }
    public string? JournalRefId { get; set; }
    public string? LoanRefId { get; set; }
    public bool IsEmpty => (LedgerRefId is null || LedgerRefId == 0)
                            && Debit == 0 && Credit == 0
                            && string.IsNullOrWhiteSpace(EntityNameRefId);

}
