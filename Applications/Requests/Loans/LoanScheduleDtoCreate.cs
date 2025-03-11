namespace Applications.Requests.Loans
{
    public class LoanScheduleDtoCreate
    {
        public int? BranchId { get; set; } 
        public long? AgentRefId { get; set; } 
        public int? InstallmentNo { get; set; }
        public int? RepaymentModeRefId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? TotalDay { get; set; } = 0;
        public decimal? Principal { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFee { get; set; } 
        public decimal? TotalPay { get; set; }
        public decimal? LoanOutstanding { get; set; }
        public int? ScheduleStatusRefId { get; set; } = 2;   
    }
}
