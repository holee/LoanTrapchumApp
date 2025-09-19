namespace Applications.Features.LoanPayments.Requests
{
    public sealed class LoanRepaymentDtoUpdate 
    {
        public long Id { get; set; }
        public long? LoanRefId { get; set; }
        public long? CreditAgentRefId { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public DateTime? PayDate { get; set; }
        public int? InstallmentNo { get; set; }
        public decimal? Principle { get; set; }
        public decimal? Interest { get; set; }
        public decimal? OtherInterest { get; set; }
        public decimal? OtherCharged { get; set; }
        public decimal? SubTotal { get; set; }
        public bool? IsReturn { get; set; }
        public ICollection<LoanRepaymentLineDtoUpdate>? LoanRepaymentLines { get; set; }

    }
}
