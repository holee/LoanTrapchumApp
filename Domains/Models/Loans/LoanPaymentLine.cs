namespace Domains.Models.Loans
{
    public sealed class LoanPaymentLine 
    {
        public long Id { get; set; }
        public string? LoanPaymentRefId { get; set; }
        public string? LoanRefId { get; set; }
        public int? BranchRefId { get; set; }
        public long? LoanScheduleRefId { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public int? InstallmentNo { get; set; }
        public int? NumberOfDay { get; set; }
        public DateTime? PayDate { get; set; }
        public int? DayArrea { get; set; }
        public decimal? PaidPrinciple { get; set; }
        public decimal? PaidInterest { get; set; }
        public decimal? PaidCollectionFee { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public decimal? SubTotal { get; set; }
        public DateTime? Created { get; set; }= DateTime.Now;
    }
}
