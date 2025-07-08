namespace Domains.Models.Loans
{
    public sealed class LoanSchedule
    {
        public long LoanScheduleId { get; set; }
        public string? LoanRefId { get; set; }
        public string? AgentRefId { get; set; }
        public int? BranchId { get; set; }
        public int? InstallmentNo { get; set; }
        public int? DaysAccrued { get; set; }
        public int? RepaymentModeRefId { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public int? TotalDay { get; set; } = 0;
        public decimal? Principal { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFee { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? LoanOutstanding { get; set; }
        public decimal? PaidPrinciple { get; set; }
        public decimal? PaidInterest { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? OtherCharge { get; set; }
        public decimal? TmpPaidPrincipal { get; set; }
        public decimal? TmpPaidInterest { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? RiskFee { get; set; }
        public decimal? PaidCollectionFee { get; set; }
        public decimal? PaidRiskFee { get; set; }
        public decimal? TmpPaidCollectionFee { get; set; }
        public decimal? TmpPaidRiskFee { get; set; }
        public bool? IsPrincipal { get; set; }
        public bool? IsInterest { get; set; }
        public string? BatchName { get; set; }
        public int? ScheduleStatusRefId { get; set; }
        public string? ScheduleStatusName { get; set; }
        public int? ScheduleNum { get; set; } = 1;
    }
}
