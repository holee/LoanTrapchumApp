namespace Applications.Responses.ViewModels.Ledgers
{
    public class LedgerTypeVm
    {
        public int? LedgerTypeId { get; set; }
        public string? LedgerTypeCode { get; set; }
        public string? CrOrDr { get; set; }
        public decimal? NOrder { get; set; }
    }
}
