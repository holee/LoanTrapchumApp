namespace Domains.Models.Loans
{
    public class LoanScheduleDetail
    {
        public long Id { get; set; }
        public long? LoanScheduleRefId { get; set; }
        public Guid? LoanRefId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public int? DaysArrear { get; set; }
        public decimal? Principal { get; set; }
        public decimal? Interest { get; set; }
        public decimal? Charge { get; set; }
        public decimal? Outstanding { get; set; }
        public string? OfficialReceipt { get; set; }
        public string? IsReversed { get; set; }
        public decimal? CollectionFee { get; set; }
        public decimal? RiskFee { get; set; }
    }
}
