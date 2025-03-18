namespace Applications.Extensions.LoanCalculators
{
    public class LoanSchedule
    {
        public DateTime Payment { get; set; }
        public int NumOfDays { get; set; }
        public decimal? BeginBalance { get; set; }
        public decimal? Principle { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFee { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? LastBalance { get; set; }
    }
}
