namespace Domains.Models.Loans
{
    public sealed class LoanRepayment
    {
        public long Id { get; set; }
        public long? TranxId { get; set; } 
        public long? LoanRefId { get; set; }
        public long? CreditAgentRefId { get; set; }
        public int? BranchRefId { get; set; }
        public long? UserRefId { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Principle { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFeeAmount { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public decimal? SubTotal { get; set; }
        public bool? IsReturn { get; set; }
        public int? CurrencyRefId { get; set; }
        public ICollection<LoanRepaymentLine>? LoanRepaymentLines { get; set; } = [];
    }
}
