namespace Applications.Responses.ViewModels.Loans.Schedules
{
    public sealed class ScheduleViewModel 
    {
        public long? LoanScheduleId { get; set; }
        public long? LoanId { get; set; }
        public long CustomerId { get; set; }
        public int? BranchId { get; set; }
        public int? InstallmentNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? TotalDay { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? Principal { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFee { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? LoanOutstanding { get; set; }
    }


}
