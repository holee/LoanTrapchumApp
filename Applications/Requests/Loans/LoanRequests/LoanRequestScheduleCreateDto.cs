namespace Applications.Requests.Loans.LoanRequests
{
    public sealed class LoanRequestScheduleCreateDto
    {
        public int? InstallmentNo { get; set; }
        public decimal? Principal { get; set; }
        public decimal? InterestAmount { get; set; }
        public decimal? CollectionFeeAmount { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? LoanOutstanding { get; set; }
    }
}
