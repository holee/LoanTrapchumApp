using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Requests.Loans.Payments
{
    public sealed class LoanPaymentLineDtoCreate 
    {
        public long? RepaymentRefId { get; set; }
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
        public decimal? PaidPenaltyAmount { get; set; }
        public decimal? SubTotal { get; set; }

        [NotMapped]
        public bool? IsEnable { get; set; }
        [NotMapped]
        public decimal? TotalWithPenalty => PaidPrinciple + PaidInterest + PaidCollectionFee + PaidPenaltyAmount ;
        [NotMapped]
        public decimal? TotalWithNoPenalty => PaidPrinciple + PaidInterest + PaidCollectionFee;
    }
}
