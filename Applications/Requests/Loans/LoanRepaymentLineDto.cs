namespace Applications.Requests.Loans
{
    public class LoanRepaymentLineDto
    {
        public long Id { get; set; }
        public long? RepaymentRefId { get; set; }
        public long? LoanRefId { get; set; }
        public long? LoanScheduleRefId { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public int? InstallmentNo { get; set; }
        public int? NumberOfDay { get; set; }
        public DateTime? PayDate { get; set; }
        public int? DayArrea { get; set; }
        public decimal? Principle { get; set; }
        public decimal? Interest { get; set; }
        public decimal? OtherInterest { get; set; }
        public decimal? OtherCharge { get; set; }
        public decimal? SubTotal { get; set; }

    }
}
