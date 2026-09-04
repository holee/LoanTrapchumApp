namespace Application.Features.Currencies.Responses
{
    public sealed class VCurrencyNotes  
    {
        public int Id { get; set; }
        public int? BranchId { get; set; }
        public int? CurrencyId { get; set; }
        public int CurNoteRefId { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public decimal? CurrencyValue { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Qty { get; set; } = 0;
        public int? TotalValue { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

}
