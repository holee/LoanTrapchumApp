using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Journals.Dtos;

public sealed class JournalEntryLineDtoCreate
{
    public long JournalLineId { get; set; }
    public string? JournalRefId { get; set; }
    public string? LoanRefId { get; set; }
    public string? EntityNameRefId { get; set; }
    public string? EntityName { get; set; } = string.Empty;
    public int? LineSeq { get; set; } = 1;
    public int? LedgerRefId { get; set; }
    public string LedgerName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? BaseAmount { get; set; }
    public string? JournalLineDesc { get; set; }
}
